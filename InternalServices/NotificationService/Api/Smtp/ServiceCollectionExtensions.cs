using CIS.InternalServices.NotificationService.Api.Configuration;

namespace CIS.InternalServices.NotificationService.Api.Smtp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmtpClient(this IServiceCollection services, SmtpConfiguration configuration)
    {
        return services;
    }
}