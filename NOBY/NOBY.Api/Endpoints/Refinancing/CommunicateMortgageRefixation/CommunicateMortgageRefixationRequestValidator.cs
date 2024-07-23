using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.CommunicateMortgageRefixation;

internal sealed class CommunicateMortgageRefixationRequestValidator 
    : AbstractValidator<CommunicateMortgageRefixationRequest>
{
    public CommunicateMortgageRefixationRequestValidator(IFeatureManager featureManager)
    {
         RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
         .WithErrorCode(90057);
    }
}
