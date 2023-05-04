using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal sealed class GetSalesArrangementRequestValidator
    : AbstractValidator<GetSalesArrangementRequest>
{
    public GetSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}