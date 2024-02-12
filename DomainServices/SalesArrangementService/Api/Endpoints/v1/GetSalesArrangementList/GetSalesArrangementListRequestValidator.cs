using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementList;

internal sealed class GetSalesArrangementListRequestValidator
    : AbstractValidator<Contracts.GetSalesArrangementListRequest>
{
    public GetSalesArrangementListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("18008");
    }
}
