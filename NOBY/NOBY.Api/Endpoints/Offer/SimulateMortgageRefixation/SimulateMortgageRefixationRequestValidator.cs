using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationRequestValidator 
    : AbstractValidator<OfferSimulateMortgageRefixationRequest>
{
    public SimulateMortgageRefixationRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
            .WithErrorCode(90057);
    }
}
