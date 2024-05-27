using CIS.InternalServices.NotificationService.Api.Validators;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsRequestValidator
    : AbstractValidator<Contracts.v2.SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberRequired)
            .Must(t => t.TryParsePhone(out _, out _))
            .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberInvalid);

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SmsProcessPriorityInvalid);

        RuleFor(request => request.Type)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTypeInvalid);

        RuleFor(request => request.Text)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.SmsTextRequired)
            .MaximumLength(480)
            .WithErrorCode(ErrorCodeMapper.SmsTextLengthLimitExceeded);

        RuleFor(request => request.Identifier)
            .SetValidator(new IdentifierValidator())
            .When(t => t.Identifier is not null)
            .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);

        RuleFor(request => request.Product)
            .SetValidator (new ProductValidator())
            .When(t => t.Product is not null)
            .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        
        RuleFor(request => request.CustomId)
            .SetValidator(new CustomIdValidator())
            .When(t => !string.IsNullOrEmpty(t.CustomId))
            .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        
        RuleFor(request => request.DocumentId)
            .SetValidator(new DocumentIdValidator())
            .When(t => !string.IsNullOrEmpty(t.DocumentId))
            .WithErrorCode(ErrorCodeMapper.DocumentIdInvalid);
    
        RuleForEach(request => request.DocumentHashes)
            .SetValidator(new DocumentHashValidator())
            .When(t => t.DocumentHashes is not null && t.DocumentHashes.Count > 0)
            .WithErrorCode(ErrorCodeMapper.DocumentHashInvalid);
    }
}
