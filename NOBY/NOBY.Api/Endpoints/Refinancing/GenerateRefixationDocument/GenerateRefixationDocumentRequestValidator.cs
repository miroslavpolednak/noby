using FluentValidation;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefixationDocument;

internal class GenerateRefixationDocumentRequestValidator : AbstractValidator<GenerateRefixationDocumentRequest>
{
    public GenerateRefixationDocumentRequestValidator()
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");
    }
}