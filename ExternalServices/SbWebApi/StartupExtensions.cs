using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using CIS.ExternalServicesHelpers;

namespace ExternalServices.SbWebApi;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddExternalServiceSbWebApi(this WebApplicationBuilder builder)
    {
        var configuration = builder.CreateAndCheckExternalServiceConfiguration<Configuration.SbWebApiConfiguration>("SbWebApi");

        switch (configuration.Version)
        {
            case Versions.V1:
                if (configuration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    builder.Services.AddScoped<V1.ISbWebApiClient, V1.MockSbWebApiClient>();
                /*else
                    builder.Services.AddHttpClient<V1.ISbWebApiClient, V1.RealMpHomeClient>(c =>
                    {
                        c.BaseAddress = new Uri(mpHomeConfiguration.ServiceUrl);
                        var byteArray = Encoding.ASCII.GetBytes($"{mpHomeConfiguration.Username}:{mpHomeConfiguration.Password}");
                        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    });*/
                break;

            default:
                throw new NotImplementedException($"SbWebApi version {configuration.Version} client not implemented");
        }

        return builder;
    }
}
