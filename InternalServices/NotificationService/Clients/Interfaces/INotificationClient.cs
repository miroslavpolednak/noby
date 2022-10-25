using CIS.Core.Results;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Clients.Interfaces;

public interface INotificationClient
{
    Task<IServiceCallResult> SendSms(SmsSendRequest request, CancellationToken token);
    Task<IServiceCallResult> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token);
    Task<IServiceCallResult> SendEmail(EmailSendRequest request, CancellationToken token);
    Task<IServiceCallResult> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token);
    Task<IServiceCallResult> GetResult(ResultGetRequest request, CancellationToken token);
}