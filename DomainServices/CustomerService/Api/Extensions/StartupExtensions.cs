using CIS.Infrastructure.StartupExtensions;
using DomainServices.CustomerService.Api.Clients;
using DomainServices.CustomerService.ExternalServices;

namespace DomainServices.CustomerService.Api.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb"));

        builder.AddCustomerManagementService();

        builder.AddExternalService<ExternalServices.CustomerManagement.V1.ICustomerManagementClient>();
        builder.AddExternalService<ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();

        // CodebookService
        builder.Services.AddCodebookService();

        return builder;
    }
}
