using DomainServices.CodebookService.Clients;
using FluentValidation;
using Microsoft.FeatureManagement;
using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<SimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator(ICodebookServiceClient codebookService, InterestRatesValidFromService ratesValidFromService, IFeatureManager featureManager)
    {
        RuleFor(t => t.FeeAmountDiscounted)
            .MustAsync(async (t, cancellationToken) => (await codebookService.FeeChangeRequests(cancellationToken)).Any(x => x.Amount == t))
            .When(t => t.FeeAmountDiscounted.HasValue);

        RuleFor(t => t.InterestRateValidFrom)
            .MustAsync(async (req, t, cancellationToken) =>
            {
                var dates = await ratesValidFromService.GetValidityDates(req.CaseId, cancellationToken);
                return dates.Date1 == t.Date || dates.Date2 == t.Date;
            })
            .When(t => t.FeeAmountDiscounted.HasValue);

        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
         .WithErrorCode(90054);
    }
}
