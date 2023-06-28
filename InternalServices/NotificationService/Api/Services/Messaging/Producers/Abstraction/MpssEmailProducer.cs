namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Abstraction;

public interface IMpssEmailProducer
{
    Task SendEmail(MpssSendApi.v1.email.SendEmail sendEmail, CancellationToken cancellationToken);
}