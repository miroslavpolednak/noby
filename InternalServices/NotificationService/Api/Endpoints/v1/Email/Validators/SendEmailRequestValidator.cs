using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.v1.Common;
using CIS.InternalServices.NotificationService.LegacyContracts.Email;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

internal sealed class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator(IOptions<AppConfiguration> options)
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorCodeMapper.FromRequired)
            .SetValidator(new EmailAddressFromValidator(options.Value))
                .WithErrorCode(ErrorCodeMapper.FromInvalid);
        
        RuleFor(request => request.To)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.ToRequired)
            .ForEach(to => to.SetValidator(new EmailAddressValidator()))
                .WithErrorCode(ErrorCodeMapper.ToInvalid);

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodeMapper.BccInvalid);

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorCodeMapper.CcInvalid);

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                    .WithErrorCode(ErrorCodeMapper.ReplyToInvalid);
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.SubjectRequired)
            .MaximumLength(400)
                .WithErrorCode(ErrorCodeMapper.SubjectInvalid);

        RuleFor(request => request.Content)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.ContentRequired)
            .SetValidator(new EmailContentValidator(options))
                .WithErrorCode(ErrorCodeMapper.ContentInvalid);

        RuleFor(request => request.Attachments.Count)
            .LessThanOrEqualTo(10)
                .WithErrorCode(ErrorCodeMapper.AttachmentsCountLimitExceeded);
        
        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorCodeMapper.AttachmentsInvalid);
        
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