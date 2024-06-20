using FluentValidation;

namespace NOBY.Api.Endpoints.RealEstateValuation.SetValuationTypeId;

internal sealed class SetValuationTypeIdRequestValidator
    : AbstractValidator<SetValuationTypeIdRequest>
{
    public SetValuationTypeIdRequestValidator()
    {
        RuleFor(t => t.ValuationTypeId)
            .Must(t => (int)t != 1)
            .WithErrorCode(90032);
    }
}
