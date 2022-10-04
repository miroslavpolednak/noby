using CIS.InternalServices.NotificationService.Contracts.Email;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendEmail;

public class SendEmailHandler : IRequestHandler<EmailSendRequest, EmailSendResponse>
{
    public async Task<EmailSendResponse> Handle(EmailSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new EmailSendResponse
        {
            NotificationId = "email notification id"
        };
    }
}