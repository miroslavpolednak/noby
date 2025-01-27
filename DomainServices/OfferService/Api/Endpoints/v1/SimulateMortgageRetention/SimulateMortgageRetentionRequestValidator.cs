﻿using DomainServices.OfferService.Contracts;
using FluentValidation;
using Microsoft.FeatureManagement;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<SimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t.SimulationInputs)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        RuleFor(t => t.BasicParameters)
            .Must(p => p != null)
            .WithErrorCode(ErrorCodeMapper.SimulationInputsIsEmpty);

        When(t => t.BasicParameters is not null, () =>
        {
            RuleFor(t => (decimal)t.BasicParameters.FeeAmount)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodeMapper.MortgageRetentionAmountNotValid);

            RuleFor(t => (decimal)t.BasicParameters.FeeAmountDiscounted!)
                .GreaterThanOrEqualTo(0)
                .When(t => t.BasicParameters.FeeAmountDiscounted != null)
                .WithErrorCode(ErrorCodeMapper.MortgageRetentionAmountIndividualPriceNotValid);
        });

        RuleFor(t => t)
             .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
             .WithMessage("Retence jsou zakázany");
    }
}
