using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

internal sealed class DocumentIdValidator 
    : AbstractValidator<string>
{
    public DocumentIdValidator()
    {
        RuleFor(documentId => documentId)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorCodeMapper.DocumentIdInvalid);
    }
}