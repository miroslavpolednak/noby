using DomainServices.OfferService.Contracts;
using FluentValidation;
using Microsoft.FeatureManagement;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentRequestValidator
    : AbstractValidator<SimulateMortgageExtraPaymentRequest>
{
    public SimulateMortgageExtraPaymentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
         .WithMessage("Mimořádná splátka je zakázaná");
    }
}
