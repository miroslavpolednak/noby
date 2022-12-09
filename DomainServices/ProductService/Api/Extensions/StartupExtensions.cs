using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Eas;
using ExternalServices;

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
    }

    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);
        // MpHome svc
        builder.AddExternalService<ExternalServices.MpHome.V1_1.IMpHomeClient>();

        // dbcontext
        builder.AddEntityFramework<Repositories.ProductServiceDbContext>(connectionStringKey: "konsDb");

        // repos
        builder.Services.AddScoped<Repositories.LoanRepository>();

        builder.Services.AddCodebookService();
            
        return builder;
    }
}
