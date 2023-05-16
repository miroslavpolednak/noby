using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Validators.Common;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Sms;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SendSmsFromTemplateRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotNull()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplatePhoneNumberRequired)
            .SetValidator(new PhoneNumberValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplatePhoneNumberInvalid);
        
        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplateProcessPriorityInvalid);
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplateTypeInvalid);

        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplatePlaceholdersRequired)
            .Must(placeholders =>
            {
                return placeholders.Select(p => p.Value).All(p => p.Length > 0);
            })
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplatePlaceholdersInvalid)
            .Must(placeholders =>
            {
                var totalCount = placeholders.Count;
                var uniqueCount = placeholders.Select(p => p.Key).Distinct().Count();

                return totalCount == uniqueCount;
            })
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SmsTemplatePlaceholdersInvalid);
        
        When(request => request.Identifier is not null, () =>
        {
            RuleFor(request => request.Identifier!)
                .SetValidator(new IdentifierValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentifierInvalid);
        });
    }
}