using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Endpoints.Common;
using CIS.InternalServices.NotificationService.LegacyContracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms.Validators;

public class SendSmsRequestValidator : AbstractValidator<SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.SmsPhoneNumberRequired)
            .SetValidator(new PhoneNumberValidator())
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

        When(request => request.Identifier is not null, () =>
        {
            RuleFor(request => request.Identifier!)
                .SetValidator(new IdentifierValidator())
                    .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);
        });
        
        When(request => request.CaseId.HasValue, () =>
        {
            RuleFor(request => request.CaseId!.Value)
                .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        });
        
        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                    .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        });
        
        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                    .WithErrorCode(ErrorCodeMapper.DocumentIdInvalid);
        });
        
        When(request => request.DocumentHash is not null, () =>
        {
            RuleFor(request => request.DocumentHash!)
                .SetValidator(new DocumentHashValidator())
                    .WithErrorCode(ErrorCodeMapper.DocumentHashInvalid);
        });
    }    
}