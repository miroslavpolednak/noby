using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSmsFromTemplate;

public class SendSmsFromTemplateHandler : IRequestHandler<SmsFromTemplateSendRequest, SmsFromTemplateSendResponse>
{
    public async Task<SmsFromTemplateSendResponse> Handle(SmsFromTemplateSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new SmsFromTemplateSendResponse
        {
            NotificationId = "sms from template notification id"
        };
    }
}