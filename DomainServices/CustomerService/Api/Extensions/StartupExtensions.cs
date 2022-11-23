using CIS.ExternalServicesHelpers;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Api.Clients;
using ExternalServices.MpHome;

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

    public static WebApplicationBuilder AddExternalServiceMpHome(this WebApplicationBuilder builder)
    {
        const string MpHomeName = "MpHome";

        var config = builder.Configuration
                            .GetRequiredSection(Constants.ExternalServicesConfigurationSectionName)
                            .GetRequiredSection(MpHomeName)
                            .Get<MpHomeConfiguration>();

        builder.Services.AddExternalServiceMpHome(config);

        return builder;
    }
}
