using CIS.InternalServices.NotificationService.Api.Configuration;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSmtpClient(this WebApplicationBuilder builder)
    {
        var smtpConfiguration = builder.GetSmtpConfiguration();
        return builder;
    }
}