using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddRepository(this WebApplicationBuilder builder)
    {
        // entity framework
        builder.AddEntityFramework<NotificationDbContext>(connectionStringKey: "nobyDb");
        
        builder.Services
            .AddScoped<INotificationRepository, NotificationRepository>();

        return builder;
    }
}