using FluentValidation;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRetentionDocument;

internal class GenerateRetentionDocumentRequestValidator : AbstractValidator<GenerateRetentionDocumentRequest>
{
    public GenerateRetentionDocumentRequestValidator()
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");
    }
}