using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailRequestValidator
    : AbstractValidator<UpdateRealEstateValuationDetailRequest>
{
    public UpdateRealEstateValuationDetailRequestValidator()
    {
        RuleFor(t => t.RealEstateStateId)
            .InclusiveBetween(1, 4)
            .When(t => t.RealEstateStateId.HasValue);
    }
}
