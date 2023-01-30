using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocumentManually;

public class SignDocumentManuallyValidator : AbstractValidator<SignDocumentManuallyRequest>
{
	public SignDocumentManuallyValidator()
	{
        RuleFor(e => e.DocumentOnSAId).NotNull().WithMessage($"{nameof(SignDocumentManuallyRequest.DocumentOnSAId)} is required");
    }
}
