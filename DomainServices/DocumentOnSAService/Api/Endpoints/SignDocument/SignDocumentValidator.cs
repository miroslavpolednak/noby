using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SignDocumentManually;

public class SignDocumentValidator : AbstractValidator<SignDocumentRequest>
{
	public SignDocumentValidator()
	{
        RuleFor(e => e.DocumentOnSAId).NotNull().WithMessage($"{nameof(SignDocumentRequest.DocumentOnSAId)} is required");

        RuleFor(e => e.SignatureTypeId).NotNull().WithMessage($"{nameof(SignDocumentRequest.SignatureTypeId)} is required");
    }
}
