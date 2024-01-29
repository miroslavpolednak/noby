using CIS.InternalServices.NotificationService.Contracts.v2;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsHandler
    : IRequestHandler<SendSmsRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendSmsRequest request, CancellationToken cancellation)
    {

    }

    public SendSmsHandler()
    {

    }
}
