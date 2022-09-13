using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.PushSmsFromTemplate;

public class PushSmsFromTemplateHandler : IRequestHandler<SmsFromTemplatePushRequest, SmsFromTemplatePushResponse>
{
    public async Task<SmsFromTemplatePushResponse> Handle(SmsFromTemplatePushRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new SmsFromTemplatePushResponse
        {
            NotificationId = "sms from template push notification id"
        };
    }
}