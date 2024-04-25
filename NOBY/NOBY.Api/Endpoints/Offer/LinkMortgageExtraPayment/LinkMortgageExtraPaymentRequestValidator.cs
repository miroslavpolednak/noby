using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

public class LinkMortgageExtraPaymentRequestValidator: AbstractValidator<LinkMortgageExtraPaymentRequest>
{
    public LinkMortgageExtraPaymentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
         .WithErrorCode(90058);
    }
}
