using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new SmsSendResponse
        {
            NotificationId = "sms notification id"
        };
    }
}