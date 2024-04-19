using FluentValidation;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal sealed class GenerateExtraPaymentDocumentRequestValidator : AbstractValidator<GenerateExtraPaymentDocumentRequest>
{
    public GenerateExtraPaymentDocumentRequestValidator()
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");
    }
}