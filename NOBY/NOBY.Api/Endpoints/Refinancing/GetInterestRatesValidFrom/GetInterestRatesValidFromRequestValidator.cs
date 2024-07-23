using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetInterestRatesValidFrom;

internal sealed class GetInterestRatesValidFromRequestValidator : AbstractValidator<GetInterestRatesValidFromRequest>
{
    public GetInterestRatesValidFromRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
        .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
        .WithErrorCode(90056);
    }
}
