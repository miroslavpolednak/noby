using CIS.Infrastructure.StartupExtensions;
using ExternalServices;

namespace DomainServices.ProductService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder)
    {
        // EAS svc
        builder.AddExternalService<global::ExternalServices.Eas.V1.IEasClient>();
        // MpHome svc
        builder.AddExternalService<global::ExternalServices.MpHome.V1.IMpHomeClient>();
        builder.AddExternalService<DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient>();

        // dbcontext
        builder.AddEntityFramework<Database.ProductServiceDbContext>(connectionStringKey: "konsDb");

        // repos
        builder.Services.AddScoped<Database.LoanRepository>();

        builder.Services.AddCodebookService();
            
        return builder;
    }
}
