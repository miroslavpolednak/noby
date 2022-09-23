using CIS.InternalServices.NotificationService.Contracts.Email;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendEmailFromTemplate;

public class SendEmailFromTemplateHandler : IRequestHandler<EmailFromTemplateSendRequest, EmailFromTemplateSendResponse>
{
    public async Task<EmailFromTemplateSendResponse> Handle(EmailFromTemplateSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new EmailFromTemplateSendResponse
        {
            NotificationId = "email from template notification id"
        };
    }
}