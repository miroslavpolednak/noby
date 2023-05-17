using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Common;

public class DocumentIdValidator : AbstractValidator<string>
{
    public DocumentIdValidator()
    {
        RuleFor(documentId => documentId)
            .Matches("^([A-Za-z0-9-_]{0,450})$")
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.DocumentIdInvalid);
    }
}