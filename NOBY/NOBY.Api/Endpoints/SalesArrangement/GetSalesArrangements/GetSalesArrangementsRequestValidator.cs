using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangements;

internal sealed class GetSalesArrangementsRequestValidator
    : AbstractValidator<GetSalesArrangementsRequest>
{
    public GetSalesArrangementsRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0).WithMessage("CaseId must be > 0");
    }
}