using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Endpoints.Common;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms.Validators;

public class SendSmsRequestValidator : AbstractValidator<SendSmsRequest>
{
    public SendSmsRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsPhoneNumberRequired)
            .SetValidator(new PhoneNumberValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsPhoneNumberInvalid);

        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsProcessPriorityInvalid);

        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTypeInvalid);

        RuleFor(request => request.Text)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTextRequired)
            .MaximumLength(480)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTextLengthLimitExceeded);

        When(request => request.Identifier is not null, () =>
        {
            RuleFor(request => request.Identifier!)
                .SetValidator(new IdentifierValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentifierInvalid);
        });
        
        When(request => request.CaseId.HasValue, () =>
        {
            RuleFor(request => request.CaseId!.Value)
                .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.CaseIdInvalid);
        });
        
        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.CustomIdInvalid);
        });
        
        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.DocumentIdInvalid);
        });
        
        When(request => request.DocumentHash is not null, () =>
        {
            RuleFor(request => request.DocumentHash!)
                .SetValidator(new DocumentHashValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.DocumentHashInvalid);
        });
    }    
}