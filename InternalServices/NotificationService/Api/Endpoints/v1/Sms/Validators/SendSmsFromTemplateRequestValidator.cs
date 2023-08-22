using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Endpoints.Common;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms.Validators;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SendSmsFromTemplateRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
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
        
        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.DocumentIdInvalid);
        });
        
        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.CustomIdInvalid);
        });
    }
}