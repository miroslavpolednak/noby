using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;

internal sealed class EmailContentValidator 
    : AbstractValidator<Contracts.v2.SendEmailRequest.Types.EmailContent>
{
    // todo: options move to error code mapper => WithErrorCode(int param)
    public EmailContentValidator()
    {
        RuleFor(content => content.Text)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.EmailTextRequired);
    }
}