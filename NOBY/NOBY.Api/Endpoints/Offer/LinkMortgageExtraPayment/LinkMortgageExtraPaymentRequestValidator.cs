using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageExtraPayment;

internal sealed class LinkMortgageExtraPaymentRequestValidator
    : AbstractValidator<OfferLinkMortgageExtraPaymentRequest>
{
    public LinkMortgageExtraPaymentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
         .WithErrorCode(90058);
    }
}
