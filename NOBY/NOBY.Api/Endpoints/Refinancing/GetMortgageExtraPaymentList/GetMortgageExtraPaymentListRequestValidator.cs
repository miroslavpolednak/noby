using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;

internal sealed class GetMortgageExtraPaymentListRequestValidator
    : AbstractValidator<GetMortgageExtraPaymentListRequest>
{
    public GetMortgageExtraPaymentListRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
            .WithErrorCode(90057);
    }
}
