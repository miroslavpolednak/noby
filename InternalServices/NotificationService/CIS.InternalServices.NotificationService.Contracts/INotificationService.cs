using System.ServiceModel;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Contracts;

[ServiceContract(Name = "CIS.InternalServices.NotificationService")]
public interface INotificationService
{
    [OperationContract]
    ValueTask<SmsPushResponse> PushSms(SmsPushRequest request, CancellationToken token);
    [OperationContract]
    ValueTask<SmsFromTemplatePushResponse> PushSmsFromTemplate(SmsFromTemplatePushRequest request, CancellationToken token);
}