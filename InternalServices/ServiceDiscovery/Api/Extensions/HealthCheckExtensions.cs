using Dapper;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            var result = new List<HealthCheckItem>(_serviceNamesCache!.Count);

            foreach (var service in _serviceNamesCache)
            {
                HealthCheckItem check = new()
                {
                    ServiceName = service,
                    Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.Unknown
                };

                var client = grpcClientFactory.CreateClient<Grpc.Health.V1.Health.HealthClient>(service);
                var timer = new Stopwatch();

                try
                {
                    var response = await client.CheckAsync(
                        new Grpc.Health.V1.HealthCheckRequest(), 
                        deadline: DateTime.UtcNow.AddSeconds(_deadlineSeconds),
                        cancellationToken: cancellationToken);

                    check.Status = response.Status;
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unimplemented)
                {
                    check.Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.NotServing;
                }
                catch (Exception ex)
                {
                    check.Status = Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus.NotServing;
                }
                finally
                {
                    timer.Stop();

                    check.Elapsed = timer.ElapsedMilliseconds;
                }

                result.Add(check);
            }

            return result;
        });

        return app;
    }

    private const int _deadlineSeconds = 5;
    private const string _sqlQuery = "SELECT ServiceName, ServiceUrl, ServiceType FROM ServiceDiscovery WHERE ServiceName!='CIS:ServiceDiscovery' AND ServiceType=1 AND EnvironmentName=@name";

    private static IReadOnlyCollection<string>? _serviceNamesCache;

    internal class HealthCheckItem
    {
        public string ServiceName { get; set; } = string.Empty;
        public Grpc.Health.V1.HealthCheckResponse.Types.ServingStatus Status { get; set; }
        public long Elapsed { get; set; }
    }
}
