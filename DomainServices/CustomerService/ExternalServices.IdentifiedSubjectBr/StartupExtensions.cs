using Microsoft.AspNetCore.Builder;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.Extensions.DependencyInjection;
using CIS.Foms.Enums;

namespace DomainServices.CustomerService.ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "IdentifiedSubjectBr";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient
        => builder.AddIdentifiedSubjectBr<TClient>(IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient.Version);

    static WebApplicationBuilder AddIdentifiedSubjectBr<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient, IdentifiedSubjectBr.V1.MockIdentifiedSubjectBrClient>();
                break;

            case (IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient, IdentifiedSubjectBr.V1.RealIdentifiedSubjectBrClient>()
                    .AddExternalServicesKbHeaders("CUSTOMER_SERVICE")
                    //.AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }
}
