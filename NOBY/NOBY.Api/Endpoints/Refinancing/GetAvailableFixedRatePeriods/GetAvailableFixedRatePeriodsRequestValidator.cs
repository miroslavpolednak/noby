using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;

internal sealed class GetAvailableFixedRatePeriodsRequestValidator: AbstractValidator<GetAvailableFixedRatePeriodsRequest>
{
    public GetAvailableFixedRatePeriodsRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
            .WithErrorCode(90057);
    }
}
