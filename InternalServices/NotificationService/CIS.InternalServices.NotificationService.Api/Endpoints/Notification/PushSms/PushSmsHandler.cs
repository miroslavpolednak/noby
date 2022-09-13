using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.PushSms;

public class PushSmsHandler : IRequestHandler<SmsPushRequest, SmsPushResponse>
{
    public async Task<SmsPushResponse> Handle(SmsPushRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new SmsPushResponse
        {
            NotificationId = "sms push notification id"
        };
    }
}