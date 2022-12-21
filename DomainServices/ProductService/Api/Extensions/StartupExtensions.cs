using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.ProductService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<ExternalServices.Eas.R21.IEasClient>();
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
