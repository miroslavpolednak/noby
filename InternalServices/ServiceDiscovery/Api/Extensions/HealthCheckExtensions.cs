using CIS.Core.Configuration;
using CIS.InternalServices.ServiceDiscovery.Api.Common;
using Grpc.Health.V1;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace CIS.InternalServices.ServiceDiscovery.Api;

internal static class HealthCheckExtensions
{
    public static WebApplication MapGlobalHealthChecks(this WebApplication app)
    {
        app.MapGet(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, async ([FromServices] ServicesMemoryCache cache, [FromServices] ICisEnvironmentConfiguration environmentConfiguration, CancellationToken cancellationToken) =>
        {
            // vsechny grpc sluzby
            var foundServices = (await cache.GetServices(new(environmentConfiguration.EnvironmentName), cancellationToken))
                .Where(t => t.ServiceType == Contracts.ServiceTypes.Grpc && t.ServiceName != environmentConfiguration.DefaultApplicationKey)
                .ToArray();

            foreach (var service in foundServices)
            {
                var channel = GrpcChannel.ForAddress(service.ServiceUrl);
                var client = new Health.HealthClient(channel);

                var response = await client.CheckAsync(new HealthCheckRequest(), cancellationToken: cancellationToken);
                var status = response.Status;
            }
        });

        return app;
    }
}
