using CIS.InternalServices.ServiceDiscovery.Api.Database;
using CIS.InternalServices.ServiceDiscovery.Contracts;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace CIS.InternalServices.ServiceDiscovery.Api;

internal static class HealthCheckExtensions
{
    public static WebApplicationBuilder AddGlobalHealthChecks(this WebApplicationBuilder builder, CIS.Core.Configuration.ICisEnvironmentConfiguration environmentConfiguration)
    {
        string? cs = builder.Configuration.GetConnectionString("default");
        // toto je pripad pro test projekt - musi se zaregistrovat ve fixture
        if (string.IsNullOrEmpty(cs))
        {
            return builder;
        }

        List<Dto.ServiceModel> services = null!;
        var optionsBuilder = new DbContextOptionsBuilder<ServiceDiscoveryDbContext>();
        optionsBuilder.UseSqlServer(cs!);
        using (var dbContext = new ServiceDiscoveryDbContext(optionsBuilder.Options))
        {
            services = dbContext.ServiceDiscoveryEntities
                .AsNoTracking()
                .Where(t => t.AddToGlobalHealthCheck && t.EnvironmentName ==  environmentConfiguration.EnvironmentName)
                .Select(t => new Dto.ServiceModel
                {
                    ServiceName = t.ServiceName,
                    ServiceType = (ServiceTypes)t.ServiceType,
                    ServiceUrl = t.ServiceUrl
                })
                .ToList();
        }

        foreach (var service in services)
        {
            builder.Services
                .AddGrpcClient<Grpc.Health.V1.Health.HealthClient>(service.ServiceName, o =>
                {
                    o.Address = new Uri(service.ServiceUrl!);
                })
                .ConfigureChannel(o =>
                {
                    HttpClientHandler httpHandler = new()
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    o.HttpHandler = httpHandler;
                });
        }

        // save service names
        _serviceNamesCache = services.Select(t => t.ServiceName).ToArray().AsReadOnly();

        return builder;
    }

    public static WebApplication MapGlobalHealthChecks(this WebApplication app)
    {
        app.MapGet(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, async (
            [FromServices] GrpcClientFactory? grpcClientFactory,
            CancellationToken cancellationToken) =>
        {
            if (grpcClientFactory is null)
            {
                return Results.Text("No services added to HealthCheck endpoint. Add services using AddToGlobalHealthCheck=1 in ServiceDiscovery table.", statusCode: (int)HttpStatusCode.NoContent);
            }
            else
            {
                var healthCheckResults = await getHealthCheckResults(grpcClientFactory, cancellationToken);
                var isAllHealthy = healthCheckResults.All(t => t.Status == Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Serving);

                return Results.Text(jsonBuilder(healthCheckResults, isAllHealthy), "application/json", isAllHealthy ? 200 : 503);
            }
        })
            .WithTags("Monitoring")
            .WithName("Globální HealthCheck endpoint sdružující HC všech služeb v NOBY.")
            .WithOpenApi();

        return app;
    }

    /// <summary>
    /// Projit vsechny endpointy healthchecku a ziskat jejich statusy
    /// </summary>
    private static async Task<List<HealthCheckResult>> getHealthCheckResults(GrpcClientFactory grpcClientFactory, CancellationToken cancellationToken)
    {
        List<HealthCheckResult> results = new();

        foreach (var service in _serviceNamesCache!)
        {
            var result = new HealthCheckResult
            {
                ServiceName = service,
                Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Unknown
            };
            results.Add(result);

            var client = grpcClientFactory.CreateClient<Grpc.Health.V1.Health.HealthClient>(service);
            var timer = new Stopwatch();

            try
            {
                timer.Start();

                var response = await client.CheckAsync(
                    new Grpc.Health.V1.HealthCheckRequest(),
                    deadline: DateTime.UtcNow.AddSeconds(_deadlineSeconds),
                    cancellationToken: cancellationToken);

                result.Status = response.Status;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unimplemented)
            {
                result.Description = "HealthCheck unimplemented";
                result.Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.NotServing;
            }
            catch (Exception ex)
            {
                result.Description = ex.Message;
                result.Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.NotServing;
            }
            finally
            {
                timer.Stop();

                result.ElapsedMilliseconds = timer.ElapsedMilliseconds;
            }
        }

        return results;
    }

    /// <summary>
    /// Vytvoreni JSON stringu na response
    /// </summary>
    private static ReadOnlySpan<byte> jsonBuilder(List<HealthCheckResult> healthCheckResults, bool healthy)
    {
        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, _jsonWriterOptions))
        {
            jsonWriter.WriteStartObject();
            
            jsonWriter.WriteString("status", healthy ? _healthyValue : _unhealthyValue);
            jsonWriter.WriteNumber("totalMilliseconds", healthCheckResults.Sum(t => t.ElapsedMilliseconds));

            jsonWriter.WriteStartObject("results");

            foreach (var results in healthCheckResults)
            {
                jsonWriter.WriteStartObject(results.ServiceName);

                jsonWriter.WriteString("status", results.Status == Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Serving ? _healthyValue : _unhealthyValue);
                
                jsonWriter.WriteNumber("totalMilliseconds", results.ElapsedMilliseconds);

                if (!string.IsNullOrEmpty(results.Description))
                {
                    jsonWriter.WriteString("description", results.Description);
                }

                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        return memoryStream.ToArray().AsSpan();
    }

    private sealed class HealthCheckResult
    {
        public string ServiceName = string.Empty;
        
        public Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus Status;
        
        public long ElapsedMilliseconds;
        
        public string? Description;
    }

    private static JsonWriterOptions _jsonWriterOptions = new JsonWriterOptions { Indented = true };
    private const string _healthyValue = "Healthy";
    private const string _unhealthyValue = "Unhealthy";
    private const int _deadlineSeconds = 2;
    
    private static IReadOnlyCollection<string>? _serviceNamesCache;
}
