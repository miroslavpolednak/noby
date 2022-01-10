using CIS.Infrastructure.Caching;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.UserService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddUserService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // db repo
        builder.Services.AddDapper(builder.Configuration.GetConnectionString("xxvvss"));

        builder.Services.AddHttpContextAccessor();
        builder.AddCisCurrentUser();

        // cache
        if (appConfiguration.Cache?.CacheType != CacheTypes.None)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (appConfiguration.Cache.UseServiceDiscovery)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            {
                builder.Services.AddRedisGlobalCache(provider =>
                {
                    string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                        .GetService(new("CIS:GlobalCache:Redis"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                        .GetAwaiter()
                        .GetResult()?
                        .ServiceUrl;
                    return url ?? throw new ArgumentNullException("url", "Service Discovery can not find CIS:GlobalCache:Redis Proprietary service URL");
                }, appConfiguration.Cache.CacheKeyPrefix);
            }
            else
            {
                if (string.IsNullOrEmpty(appConfiguration.Cache.CacheConnectionString))
                    throw new ArgumentNullException("CacheConnectionString", "Redis connection string for Service Discovery Global Cache must be defined");
                builder.Services.AddRedisGlobalCache(appConfiguration.Cache.CacheConnectionString, appConfiguration.Cache.CacheKeyPrefix);
            }
        }

        return builder;
    }
}
