using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsOnSAList;

public class GetDocumentsOnSAListValidator : AbstractValidator<GetDocumentsOnSAListRequest>
{
	public GetDocumentsOnSAListValidator()
	{
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(GetDocumentsToSignListRequest.SalesArrangementId)} is required");
    }
}
