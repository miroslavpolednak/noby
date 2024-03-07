using DomainServices.CodebookService.Clients;
using FluentValidation;
using NOBY.Services.InterestRatesValidFrom;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<SimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator(ICodebookServiceClient codebookService, InterestRatesValidFromService ratesValidFromService)
    {
        RuleFor(t => t.FeeAmountIndividualPrice)
            .MustAsync(async (t, cancellationToken) => (await codebookService.FeeChangeRequests(cancellationToken)).Any(x => x.Amount == t))
            .When(t => t.FeeAmountIndividualPrice.HasValue);

        RuleFor(t => t.InterestRateValidFrom)
            .MustAsync(async (req, t, cancellationToken) => 
            {
                var dates = await ratesValidFromService.GetValidityDates(req.CaseId, cancellationToken);
                return dates.Date1 == t || dates.Date2 == t;
            })
            .When(t => t.FeeAmountIndividualPrice.HasValue);
    }
}
