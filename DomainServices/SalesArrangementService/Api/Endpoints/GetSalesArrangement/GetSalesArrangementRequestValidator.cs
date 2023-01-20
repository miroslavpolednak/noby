using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangement;

internal class GetSalesArrangementRequestValidator
    : AbstractValidator<Contracts.GetSalesArrangementRequest>
{
    public GetSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("18010");
    }
}
