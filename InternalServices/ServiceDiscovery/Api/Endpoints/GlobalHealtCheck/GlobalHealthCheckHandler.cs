using CIS.Core.Types;
using CIS.InternalServices.ServiceDiscovery.Api.Common;
using Grpc.Core;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace CIS.InternalServices.ServiceDiscovery.Api.Endpoints.GlobalHealtCheck;

[Core.Attributes.ScopedService, Core.Attributes.SelfService]
internal sealed class GlobalHealthCheckHandler
{
    private static JsonWriterOptions _jsonWriterOptions = new JsonWriterOptions { Indented = true };
    private const string _healthyValue = "Healthy";
    private const string _unhealthyValue = "Unhealthy";
    private const int _deadlineSeconds = 2;

    private readonly ServicesMemoryCache _cache;
    private readonly Core.Configuration.ICisEnvironmentConfiguration _environmentConfiguration;

    public GlobalHealthCheckHandler(ServicesMemoryCache cache, Core.Configuration.ICisEnvironmentConfiguration environmentConfiguration)
    {
        _environmentConfiguration = environmentConfiguration;
        _cache = cache;
    }

    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        var services = await _cache.GetServices(new ApplicationEnvironmentName(_environmentConfiguration.EnvironmentName), cancellationToken);

        if ((services?.Count ?? 0) == 0)
        {
            return Results.Text($"No services found for current environment ({_environmentConfiguration.EnvironmentName}).", statusCode: (int)HttpStatusCode.NotFound);
        }
        if (!services!.Any(t => t.AddToGlobalHealthCheck))
        {
            return Results.Text("No services added to HealthCheck endpoint. Add services using AddToGlobalHealthCheck=1 in ServiceDiscovery table.", statusCode: (int)HttpStatusCode.NoContent);
        }
        else
        {
            var servicesToCheck = services!.Where(t => t.AddToGlobalHealthCheck);
            var healthCheckResults = await getHealthCheckResults(servicesToCheck, cancellationToken);
            var isAllHealthy = healthCheckResults.All(t => t.Status == Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Serving);

            return Results.Text(jsonBuilder(healthCheckResults, isAllHealthy), "application/json", isAllHealthy ? 200 : 503);
        }
    }

    /// <summary>
    /// Projit vsechny endpointy healthchecku a ziskat jejich statusy
    /// </summary>
    private static async Task<List<GlobalHealthCheckResult>> getHealthCheckResults(IEnumerable<Contracts.DiscoverableService> services, CancellationToken cancellationToken)
    {
        List<GlobalHealthCheckResult> results = new();

        foreach (var service in services)
        {
            var result = new GlobalHealthCheckResult
            {
                ServiceName = service.ServiceName,
                Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Unknown
            };
            results.Add(result);

            var timer = new Stopwatch();
            try
            {
                var channel = createChannel(service);

                // pokud se nepodarilo vytvorit grpc channel
                if (channel is null)
                {
                    timer.Stop();
                    result.Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.NotServing;
                    result.Description = $"Can not open GrpcChannel at {service.ServiceUrl}";
                    continue;
                }

                var client = new Grpc.Health.V1.Health.HealthClient(channel);
            
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

    private static Grpc.Net.Client.GrpcChannel? createChannel(Contracts.DiscoverableService service)
    {
        try
        {
            return GrpcChannelsCache.GetChannel(service);
        }
        catch
        {
            GrpcChannelsCache.RemoveChannel(service.ServiceName);
            return GrpcChannelsCache.GetChannel(service);
        }
    }

    /// <summary>
    /// Vytvoreni JSON stringu na response
    /// </summary>
    private static ReadOnlySpan<byte> jsonBuilder(List<GlobalHealthCheckResult> healthCheckResults, bool healthy)
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
}
