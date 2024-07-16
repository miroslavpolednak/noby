using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

public class StartSigningValidator : AbstractValidator<DocumentOnSAStartSigningRequest>
{
    public StartSigningValidator()
    {
        RuleFor(e => e.DocumentTypeId).NotNull().WithMessage($"{nameof(DocumentOnSAStartSigningRequest.DocumentTypeId)} is required");
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(DocumentOnSAStartSigningRequest.SalesArrangementId)} is required");
    }
}
