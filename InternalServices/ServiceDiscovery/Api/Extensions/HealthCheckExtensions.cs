using Dapper;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace CIS.InternalServices.ServiceDiscovery.Api;

internal static class HealthCheckExtensions
{
    public static WebApplicationBuilder AddGlobalHealthChecks(this WebApplicationBuilder builder, CIS.Core.Configuration.ICisEnvironmentConfiguration environmentConfiguration)
    {
        var connectionString = builder.Configuration.GetConnectionString("default")!;
        using var connection = (new CIS.Infrastructure.Data.SqlConnectionProvider(connectionString)).Create();
        var services = connection.Query<Dto.ServiceModel>(_sqlQuery, new { name = environmentConfiguration.EnvironmentName }).ToList();
        
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
            [FromServices] GrpcClientFactory grpcClientFactory,
            CancellationToken cancellationToken) =>
        {
            return Results.Text(await jsonBuilder(grpcClientFactory, cancellationToken), contentType: "application/json");
        });

        return app;
    }

    private static async Task<string> jsonBuilder(
        GrpcClientFactory grpcClientFactory,
        CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, _jsonWriterOptions))
        {
            jsonWriter.WriteStartObject();

            foreach (var service in _serviceNamesCache!)
            {
                jsonWriter.WriteStartObject(service);

                var client = grpcClientFactory.CreateClient<Grpc.Health.V1.Health.HealthClient>(service);
                var timer = new Stopwatch();

                try
                {
                    timer.Start();

                    var response = await client.CheckAsync(
                        new Grpc.Health.V1.HealthCheckRequest(),
                        deadline: DateTime.UtcNow.AddSeconds(_deadlineSeconds),
                        cancellationToken: cancellationToken);

                    jsonWriter.WriteString("status", response.Status == Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Serving ? _healthyValue : _unhealthyValue);
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unimplemented)
                {
                    jsonWriter.WriteString("status", _unhealthyValue);
                    jsonWriter.WriteString("description", "HealthCheck unimplemented");
                }
                catch (Exception ex)
                {
                    jsonWriter.WriteString("status", _unhealthyValue);
                    jsonWriter.WriteString("description", ex.Message);
                }
                finally
                {
                    timer.Stop();

                    jsonWriter.WriteNumber("totalMilliseconds", timer.ElapsedMilliseconds);
                }

                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
        }

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static JsonWriterOptions _jsonWriterOptions = new JsonWriterOptions { Indented = true };
    private const string _healthyValue = "Healthy";
    private const string _unhealthyValue = "Unhealthy";
    private const int _deadlineSeconds = 5;
    private const string _sqlQuery = "SELECT ServiceName, ServiceUrl, ServiceType FROM ServiceDiscovery WHERE AddToGlobalHealthCheck=1 AND EnvironmentName=@name";

    private static IReadOnlyCollection<string>? _serviceNamesCache;
}
