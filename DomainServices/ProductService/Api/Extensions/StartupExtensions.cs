using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.ProductService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.V1.IEasClient>();
        // MpHome svc
        builder.AddExternalService<ExternalServices.MpHome.V1_1.IMpHomeClient>();

        // dbcontext
        builder.AddEntityFramework<Database.ProductServiceDbContext>(connectionStringKey: "konsDb");

        // repos
        builder.Services.AddScoped<Database.LoanRepository>();

        builder.Services.AddCodebookService();
            
        return builder;
    }
}
