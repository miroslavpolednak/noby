using System.ServiceModel;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Contracts;

[ServiceContract(Name = "CIS.InternalServices.NotificationService")]
public interface INotificationService
{
    [OperationContract]
    Task<SmsSendResponse> SendSms(SmsSendRequest request, CancellationToken token);
    
    [OperationContract]
    Task<SmsFromTemplateSendResponse> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token);
    
    [OperationContract]
    Task<EmailSendResponse> SendEmail(EmailSendRequest request, CancellationToken token);

    [OperationContract]
    Task<EmailFromTemplateSendResponse> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token);

    [OperationContract]
    Task<ResultGetResponse> GetResult(ResultGetRequest request, CancellationToken token);

    [OperationContract]
    Task<ResultsSearchByResponse> SearchResults(ResultsSearchByRequest request, CancellationToken token);
}