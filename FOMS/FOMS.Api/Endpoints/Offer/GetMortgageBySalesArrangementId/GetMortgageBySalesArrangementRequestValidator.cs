using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.Validators;

internal class GetMortgageBySalesArrangementRequestValidator
    : AbstractValidator<Dto.GetMortgageBySalesArrangementRequest>
{
    public GetMortgageBySalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}