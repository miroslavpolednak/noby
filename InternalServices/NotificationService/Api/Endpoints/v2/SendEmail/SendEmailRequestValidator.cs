using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail.Validators;
using CIS.InternalServices.NotificationService.Api.Validators;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail;

internal sealed class SendEmailRequestValidator
    : AbstractValidator<Contracts.v2.SendEmailRequest>
{
    public SendEmailRequestValidator(AppConfiguration appConfiguration)
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorCodeMapper.FromRequired)
            .SetValidator(new EmailAddressFromValidator(appConfiguration))
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
            .SetValidator(new EmailContentValidator())
                .WithErrorCode(ErrorCodeMapper.ContentInvalid);

        RuleFor(request => request.Attachments.Count)
            .LessThanOrEqualTo(10)
                .WithErrorCode(ErrorCodeMapper.AttachmentsCountLimitExceeded);

        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorCodeMapper.AttachmentsInvalid);

        RuleFor(request => request.Identifier)
            .SetValidator(new IdentifierValidator())
            .When(t => t.Identifier is not null)
            .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);

        RuleFor(request => request.Product)
            .SetValidator(new ProductValidator())
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
