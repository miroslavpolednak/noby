using CIS.Infrastructure.StartupExtensions;
using ExternalServicesTcp.Configuration;
using ExternalServicesTcp.V1.Clients;
using ExternalServicesTcp.V1.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServicesTcp
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddExternalServiceTcp(this IServiceCollection services, TcpConfiguration sdfConfiguration)
        {
            services
            .AddDapperOracle<Data.ITcpDapperConnectionProvider>(sdfConfiguration.Connectionstring);

            services.AddScoped<IDocumentServiceRepository, DocumentServiceRepository>();

            services.AddHttpClient<ITcpClient, TcpClient>();

            return services;
        }
    }
}
