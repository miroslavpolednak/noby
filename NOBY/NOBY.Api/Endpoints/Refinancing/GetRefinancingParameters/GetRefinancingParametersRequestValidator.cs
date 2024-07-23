using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;

internal sealed class GetRefinancingParametersRequestValidator : AbstractValidator<GetRefinancingParametersRequest>
{
    public GetRefinancingParametersRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention) || await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
            .WithMessage("Retence nebo refixace musejí být povoleny");
    }
}
