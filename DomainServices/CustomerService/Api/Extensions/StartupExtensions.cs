using CIS.Infrastructure.StartupExtensions;
using DomainServices.CustomerService.Api.Clients;

namespace DomainServices.CustomerService.Api.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb"));

        builder.AddCustomerManagementService();

        // CodebookService
        builder.Services.AddCodebookService();

        return builder;
    }
}
