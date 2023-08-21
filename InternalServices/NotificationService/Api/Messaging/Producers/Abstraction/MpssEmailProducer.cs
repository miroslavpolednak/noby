namespace CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;

public interface IMpssEmailProducer
{
    Task SendEmail(MpssSendApi.v1.email.SendEmail sendEmail, CancellationToken cancellationToken);
}