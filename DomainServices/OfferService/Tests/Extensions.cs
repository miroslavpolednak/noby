using CIS.Infrastructure.gRPC;
using CIS.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.OfferService.Tests;

internal static class Extensions
{
    public static IServiceCollection AddTestServices(this IServiceCollection services, TestFixture<Program> testFixture)
    {
        // unregister original dbcontext
        var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(Api.Repositories.OfferServiceDbContext));
        if (context != null)
        {
            services.Remove(context);
            var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
            foreach (var option in options)
                services.Remove(option);
        }
        // register new dbcontext
        services.AddDbContext<Api.Repositories.OfferServiceDbContext>(options => options.UseSqlite(testFixture.DatabaseFixture?.Provider.ConnectionString), ServiceLifetime.Transient, ServiceLifetime.Transient);

        // uri settings
        var uriSettingsService = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(GrpcServiceUriSettings<Abstraction.IOfferServiceAbstraction>));
        if (uriSettingsService is not null)
            services.Remove(uriSettingsService);
        //TODO jak udelat aby to tady nebylo natvrdo?
        services.AddGrpcServiceUriSettings<Abstraction.IOfferServiceAbstraction>("https://localhost/", true);

        return services;
    }
}
