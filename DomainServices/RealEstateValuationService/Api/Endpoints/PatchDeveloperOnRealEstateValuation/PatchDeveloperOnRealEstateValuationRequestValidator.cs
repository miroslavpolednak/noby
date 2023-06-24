using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.PatchDeveloperOnRealEstateValuation;

internal sealed class PatchDeveloperOnRealEstateValuationRequestValidator
    : AbstractValidator<PatchDeveloperOnRealEstateValuationRequest>
{
    public PatchDeveloperOnRealEstateValuationRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
