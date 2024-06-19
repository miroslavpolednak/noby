using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRate;

internal sealed class GetInterestRateRequestValidator : AbstractValidator<GetInterestRateRequest>
{
    public GetInterestRateRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
         .WithErrorCode(90056);
    }
}
