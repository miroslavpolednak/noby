using System.ServiceModel;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Contracts;

[ServiceContract(Name = "CIS.InternalServices.NotificationService")]
public interface INotificationService
{
    [OperationContract]
    ValueTask<SmsSendResponse> SendSms(SmsSendRequest request, CancellationToken token);
    
    [OperationContract]
    ValueTask<SmsFromTemplateSendResponse> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token);
    
    [OperationContract]
    ValueTask<EmailSendResponse> SendEmail(EmailSendRequest request, CancellationToken token);

    [OperationContract]
    ValueTask<EmailFromTemplateSendResponse> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token);

    [OperationContract]
    ValueTask<ResultGetResponse> GetResult(ResultGetRequest request, CancellationToken token);
}