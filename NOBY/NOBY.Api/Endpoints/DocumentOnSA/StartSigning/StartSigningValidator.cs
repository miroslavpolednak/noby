using FluentValidation;

namespace NOBY.Api.Endpoints.DocumentOnSA.StartSigning;

public class StartSigningValidator : AbstractValidator<StartSigningRequest>
{
	public StartSigningValidator()
	{
        RuleFor(e => e.DocumentTypeId).NotNull().WithMessage($"{nameof(StartSigningRequest.DocumentTypeId)} is required");
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(StartSigningRequest.SalesArrangementId)} is required");
	}
}
