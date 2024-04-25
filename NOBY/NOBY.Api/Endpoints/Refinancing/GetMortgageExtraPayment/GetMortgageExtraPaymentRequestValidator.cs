using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;

internal sealed class GetMortgageExtraPaymentRequestValidator : AbstractValidator<GetMortgageExtraPaymentRequest>
{
    public GetMortgageExtraPaymentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
            .WithErrorCode(90056);
    }
}
