using DomainServices.DocumentOnSAService.Contracts;
using FluentValidation;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.GetDocumentsToSignList;

public class GetDocumentsToSignListValidator : AbstractValidator<GetDocumentsToSignListRequest>
{
	public GetDocumentsToSignListValidator()
	{
        RuleFor(e => e.SalesArrangementId).NotNull().WithMessage($"{nameof(GetDocumentsToSignListRequest.SalesArrangementId)} is required");
    }
}
