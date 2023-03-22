using CIS.Core.Configuration;
using CIS.InternalServices.ServiceDiscovery.Api.Common;
using Grpc.Health.V1;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CIS.InternalServices.ServiceDiscovery.Api;

internal class HealthCheckItem
{
    public string ServiceName { get; set; } = string.Empty;
    public HealthCheckResponse.Types.ServingStatus Status { get; set; }
    public long Elapsed { get; set; }
}

internal static class HealthCheckExtensions
{
    private const int _deadlineSeconds = 5;

    public static WebApplication MapGlobalHealthChecks(this WebApplication app)
    {
        app.MapGet(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, async ([FromServices] ServicesMemoryCache cache, [FromServices] ICisEnvironmentConfiguration environmentConfiguration, CancellationToken cancellationToken) =>
        {
            // vsechny grpc sluzby
            var foundServices = (await cache.GetServices(new(environmentConfiguration.EnvironmentName), cancellationToken))
                .Where(t => t.ServiceType == Contracts.ServiceTypes.Grpc && t.ServiceName != environmentConfiguration.DefaultApplicationKey)
                .ToArray();
            var result = new List<HealthCheckItem>(foundServices.Length);

            foreach (var service in foundServices)
            {
                HealthCheckItem check = new()
                {
                    ServiceName = service.ServiceName,
                    Status = HealthCheckResponse.Types.ServingStatus.Unknown
                };

                var channel = GrpcChannel.ForAddress(service.ServiceUrl);
                var client = new Health.HealthClient(channel);

                var timer = new Stopwatch();
                try
                {
                    var response = await client.CheckAsync(
                        new HealthCheckRequest(), 
                        deadline: DateTime.UtcNow.AddSeconds(_deadlineSeconds),
                        cancellationToken: cancellationToken);
                    check.Status = response.Status;
                }
                finally
                {
                    timer.Stop();

                    check.Status = HealthCheckResponse.Types.ServingStatus.NotServing;
                    check.Elapsed = timer.ElapsedMilliseconds;
                }

                result.Add(check);
            }

            return result;
        });

        return app;
    }
}
