using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.SetACVRealEstateTypeByRealEstateValuation;

internal sealed class SetACVRealEstateTypeByRealEstateValuationRequestValidator
    : AbstractValidator<SetACVRealEstateTypeByRealEstateValuationRequest>
{
    public SetACVRealEstateTypeByRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);

        RuleFor(t => t.ACVRealEstateType)
            .NotEmpty()
            .Length(2)
            .WithErrorCode(ErrorCodeMapper.ACVRealEstateTypeIsEmpty);
    }
}
