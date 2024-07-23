using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;

internal sealed class UpdateMortgageRefixationRequestValidator
    : AbstractValidator<RefinancingUpdateMortgageRefixationRequest>
{
    public UpdateMortgageRefixationRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
           .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
           .WithErrorCode(90057);
    }
}
