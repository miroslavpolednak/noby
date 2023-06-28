using CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Services.Smtp;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSmtpClient(this WebApplicationBuilder builder)
    {
        // var smtpConfiguration = builder.GetSmtpConfiguration();
        builder.Services.AddScoped<ISmtpAdapterService, SmtpAdapterService>();
        
        return builder;
    }
}