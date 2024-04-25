using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.LinkMortgageRetentionOffer;

public class LinkMortgageRetentionOfferRequestValidator : AbstractValidator<LinkMortgageRetentionOfferRequest>
{
    public LinkMortgageRetentionOfferRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
            .WithErrorCode(90056);
    }
}
