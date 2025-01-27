﻿using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;

internal sealed class SimulateMortgageRefixationOfferListRequestValidator 
    : AbstractValidator<OfferSimulateMortgageRefixationOfferListRequest>
{
    public SimulateMortgageRefixationOfferListRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
            .WithErrorCode(90057);
    }
}
