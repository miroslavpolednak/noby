using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageRetention;

internal sealed class SimulateMortgageRetentionRequestValidator
    : AbstractValidator<SimulateMortgageRetentionRequest>
{
    public SimulateMortgageRetentionRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.FeeAmountIndividualPrice)
            .MustAsync(async (t, cancellationToken) => (await codebookService.FeeChangeRequests(cancellationToken)).Any(x => x.Amount == t))
            .When(t => t.FeeAmountIndividualPrice.HasValue);
    }
}
