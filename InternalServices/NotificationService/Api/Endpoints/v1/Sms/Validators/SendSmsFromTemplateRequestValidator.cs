using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Endpoints.Common;
using CIS.InternalServices.NotificationService.LegacyContracts.Sms;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms.Validators;

public class SendSmsFromTemplateRequestValidator : AbstractValidator<SendSmsFromTemplateRequest>
{
    public SendSmsFromTemplateRequestValidator()
    {
        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.SmsTemplatePhoneNumberRequired)
            .SetValidator(new PhoneNumberValidator())
                .WithErrorCode(ErrorCodeMapper.SmsTemplatePhoneNumberInvalid);
        
        RuleFor(request => request.ProcessingPriority)
            .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.SmsTemplateProcessPriorityInvalid);
        
        RuleFor(request => request.Type)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.SmsTemplateTypeInvalid);

        RuleFor(request => request.Placeholders)
            .NotNull()
                .WithErrorCode(ErrorCodeMapper.SmsTemplatePlaceholdersRequired)
            .Must(placeholders =>
            {
                return placeholders.Select(p => p.Value).All(p => p.Length > 0);
            })
                .WithErrorCode(ErrorCodeMapper.SmsTemplatePlaceholdersInvalid)
            .Must(placeholders =>
            {
                var totalCount = placeholders.Count;
                var uniqueCount = placeholders.Select(p => p.Key).Distinct().Count();

                return totalCount == uniqueCount;
            })
                .WithErrorCode(ErrorCodeMapper.SmsTemplatePlaceholdersInvalid);

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