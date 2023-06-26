namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Abstraction;

public interface IMcsSmsProducer
{ 
    Task SendSms(McsSendApi.v4.sms.SendSMS sendSms, CancellationToken cancellationToken);
}