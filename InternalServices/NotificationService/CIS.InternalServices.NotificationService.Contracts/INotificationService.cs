using System.ServiceModel;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Contracts;

[ServiceContract(Name = "CIS.InternalServices.NotificationService")]
public interface INotificationService
{
    Task<SmsPushResponse> PushSms(SmsPushRequest request, CancellationToken token);
    Task<SmsFromTemplatePushResponse> PushSmsFromTemplate(SmsFromTemplatePushRequest request, CancellationToken token);
}