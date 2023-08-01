using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PreorderOnlineValuation;

internal sealed class PreorderOnlineValuationRequestValidator
    : AbstractValidator<PreorderOnlineValuationRequest>
{
    public PreorderOnlineValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
