using CIS.Infrastructure.StartupExtensions;
using DomainServices.CustomerService.ExternalServices;

namespace DomainServices.CustomerService.Api.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb")!);

        builder.AddExternalService<ExternalServices.CustomerManagement.V1.ICustomerManagementClient>();
        builder.AddExternalService<ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient>();
        builder.AddExternalService<ExternalServices.CustomerProfile.V1.ICustomerProfileClient>();
        builder.AddExternalService<ExternalServices.Kyc.V1.IKycClient>();

        // CodebookService
        builder.Services.AddCodebookService();

        return builder;
    }
}
