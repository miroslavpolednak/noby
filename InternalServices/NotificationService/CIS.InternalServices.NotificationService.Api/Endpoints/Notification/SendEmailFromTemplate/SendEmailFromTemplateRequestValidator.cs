using CIS.InternalServices.NotificationService.Contracts.Email;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendEmailFromTemplate;

public class SendEmailFromTemplateRequestValidator : AbstractValidator<EmailFromTemplateSendRequest>
{
    public SendEmailFromTemplateRequestValidator()
    {
        // todo:
    }    
}