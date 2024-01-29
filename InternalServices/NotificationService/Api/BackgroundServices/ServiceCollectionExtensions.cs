using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;
using CIS.InternalServices.NotificationService.Api.BackgroundServices.SetExpiredEmails;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddBackroundJobs(this WebApplicationBuilder builder)
    {
        // odeslani MPSS emailu
        builder.AddCisBackgroundService<SendEmailsJob, SendEmailsJobConfiguration>();

        // zruseni odesilani MPSS emailu po expiraci platnosti
        builder.AddCisBackgroundService<SetExpiredEmailsJob, SetExpiredEmailsJobConfiguration>();

        return builder;
    }
}
