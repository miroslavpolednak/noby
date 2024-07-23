using DomainServices.CodebookService.Clients;
using FluentValidation;
using Microsoft.FeatureManagement;
using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<OfferSimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator(ICodebookServiceClient codebookService, InterestRatesValidFromService ratesValidFromService, IFeatureManager featureManager)
    {
        RuleFor(t => t.FeeAmountDiscounted)
            .MustAsync(async (t, cancellationToken) => (await codebookService.FeeChangeRequests(cancellationToken)).Any(x => x.Amount == t))
            .When(t => t.FeeAmountDiscounted.HasValue);

        RuleFor(t => t.InterestRateValidFrom)
            .MustAsync(async (req, t, cancellationToken) =>
            {
                var (d1, d2) = await ratesValidFromService.GetValidityDates(req.CaseId, cancellationToken);
                return d1 == t || d2 == t;
            })
            .When(t => t.FeeAmountDiscounted.HasValue);

        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
         .WithErrorCode(90056);
    }
}
