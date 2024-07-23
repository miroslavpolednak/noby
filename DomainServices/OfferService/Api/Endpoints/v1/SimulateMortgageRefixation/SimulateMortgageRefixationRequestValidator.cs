using DomainServices.OfferService.Contracts;
using FluentValidation;
using Microsoft.FeatureManagement;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationRequestValidator
    : AbstractValidator<SimulateMortgageRefixationRequest>
{
    public SimulateMortgageRefixationRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t.BasicParameters)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t)
           .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
           .WithMessage("Refixace jsou zakázany");
    }
}
