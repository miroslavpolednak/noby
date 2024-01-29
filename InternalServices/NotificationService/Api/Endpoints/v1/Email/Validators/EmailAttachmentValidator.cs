﻿using CIS.InternalServices.NotificationService.LegacyContracts.Email.Dto;
using FluentValidation;
using NOBY.Infrastructure.ErrorHandling;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email.Validators;

public class EmailAttachmentValidator : AbstractValidator<EmailAttachment>
{
    public EmailAttachmentValidator()
    {
        RuleFor(attachment => attachment.Binary)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.BinaryRequired)
            .Matches("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$")
                .WithErrorCode(ErrorCodeMapper.BinaryInvalid);
                
        RuleFor(attachment => attachment.Filename)
            .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.FilenameRequired)
            .MaximumLength(255)
                .WithErrorCode(ErrorCodeMapper.FilenameLengthLimitExceeded);
    }
}