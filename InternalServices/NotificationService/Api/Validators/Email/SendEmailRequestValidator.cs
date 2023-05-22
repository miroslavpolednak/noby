using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Validators.Common;
using CIS.InternalServices.NotificationService.Contracts.Email;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Validators.Email;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator(IOptions<AppConfiguration> options)
    {
        RuleFor(request => request.From)
            .NotNull()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FromRequired)
            .SetValidator(new EmailAddressFromValidator(options))
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.FromInvalid);
        
        RuleFor(request => request.To)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ToNotEmpty)
            .ForEach(to => to.SetValidator(new EmailAddressValidator()))
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ToInvalid);

        RuleForEach(request => request.Bcc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.BccInvalid);

        RuleForEach(request => request.Cc)
            .SetValidator(new EmailAddressValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.CcInvalid);

        When(request => request.ReplyTo is not null, () =>
        {
            RuleFor(request => request.ReplyTo!)
                .SetValidator(new EmailAddressValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.ReplyToInvalid);
        });

        RuleFor(request => request.Subject)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SubjectRequired)
            .MaximumLength(400)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.SubjectInvalid);

        RuleFor(request => request.Content)
            .NotEmpty()
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ContentRequired)
            .SetValidator(new EmailContentValidator(options))
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.ContentInvalid);

        RuleFor(request => request.Attachments.Count)
            .LessThanOrEqualTo(10)
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.AttachmentsCountLimitExceeded);
        
        RuleForEach(request => request.Attachments)
            .SetValidator(new EmailAttachmentValidator())
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.AttachmentsInvalid);
        
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