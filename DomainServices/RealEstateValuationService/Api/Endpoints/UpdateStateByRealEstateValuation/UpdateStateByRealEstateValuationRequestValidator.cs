using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateStateByRealEstateValuation;

internal sealed class UpdateStateByRealEstateValuationRequestValidator
    : AbstractValidator<UpdateStateByRealEstateValuationRequest>
{
    public UpdateStateByRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
