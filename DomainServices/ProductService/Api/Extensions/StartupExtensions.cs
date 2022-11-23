using CIS.Infrastructure.StartupExtensions;
using DomainServices.CodebookService.Clients;
using ExternalServices.Eas;
using ExternalServices.MpHome;

namespace DomainServices.ProductService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException("AppConfiguration");
        if (configuration.EAS == null)
            throw new ArgumentNullException("AppConfiguration.EAS");
        if (configuration.MpHome == null)
            throw new ArgumentNullException("AppConfiguration.MPHome");
    }

    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);
        // MpHome svc
        builder.Services.AddExternalServiceMpHome(appConfiguration.MpHome);

        // dbcontext
        builder.AddEntityFramework<Repositories.ProductServiceDbContext>("konsDb");

        // repos
        builder.Services.AddScoped<Repositories.LoanRepository>();

        builder.Services.AddCodebookService();
            
        return builder;
    }
}
