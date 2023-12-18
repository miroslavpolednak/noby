using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateValuationTypeByRealEstateValuation;

internal sealed class UpdateValuationTypeByRealEstateValuationRequestValidator
    : AbstractValidator<UpdateValuationTypeByRealEstateValuationRequest>
{
    public UpdateValuationTypeByRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
