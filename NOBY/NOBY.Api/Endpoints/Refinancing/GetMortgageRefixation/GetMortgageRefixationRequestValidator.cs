using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationRequestValidator : AbstractValidator<GetMortgageRefixationRequest>
{
    public GetMortgageRefixationRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
            .WithErrorCode(90055);
    }

}
