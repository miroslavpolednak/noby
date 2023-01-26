using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentOnSAData;

public class GetDocumentOnSADataValidator: AbstractValidator<GetDocumentOnSADataRequest>
{
	public GetDocumentOnSADataValidator()
	{
        RuleFor(e => e.DocumentOnSAId).NotNull().WithMessage($"{nameof(GetDocumentOnSADataRequest.DocumentOnSAId)} is required");
    }
}
