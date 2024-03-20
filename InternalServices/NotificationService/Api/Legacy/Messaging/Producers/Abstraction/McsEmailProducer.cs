namespace CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;

public interface IMcsEmailProducer
{
    Task SendEmail(McsSendApi.v4.email.SendEmail sendEmail, CancellationToken cancellationToken);
}