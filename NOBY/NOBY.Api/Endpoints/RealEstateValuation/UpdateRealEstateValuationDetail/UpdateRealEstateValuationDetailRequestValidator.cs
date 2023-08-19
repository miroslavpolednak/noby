using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

internal sealed class UpdateRealEstateValuationDetailRequestValidator
    : AbstractValidator<UpdateRealEstateValuationDetailRequest>
{
    public UpdateRealEstateValuationDetailRequestValidator()
    {
        RuleFor(t => t.RealEstateStateId)
            .Must(t => !t.HasValue || (t.Value >= 1 && t.Value <= 4));
    }
}
