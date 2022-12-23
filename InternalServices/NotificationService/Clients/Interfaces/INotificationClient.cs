using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Clients.Interfaces;

public interface INotificationClient
{
    Task<SmsSendResponse> SendSms(SmsSendRequest request, CancellationToken token);
    Task<SmsFromTemplateSendResponse> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token);
    Task<EmailSendResponse> SendEmail(EmailSendRequest request, CancellationToken token);
    Task<EmailFromTemplateSendResponse> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token);
    Task<ResultGetResponse> GetResult(ResultGetRequest request, CancellationToken token);
}