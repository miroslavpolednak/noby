using DomainServices.CodebookService.Clients;
using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentRequestValidator
    : AbstractValidator<SimulateMortgageExtraPaymentRequest>
{
    public SimulateMortgageExtraPaymentRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.ExtraPaymentAmount)
            .GreaterThan(0);

        RuleFor(t => t.ExtraPaymentReasonId)
            .MustAsync(async (id, cancellationToken) => (await codebookService.ExtraPaymentReasons(cancellationToken)).Any(t => t.Id == id));

        RuleFor(t => t.ExtraPaymentDate)
            .MustAsync(async (d, cancellationToken) => (await codebookService.GetNonBankingDays(DateOnly.FromDateTime(d), DateOnly.FromDateTime(d), cancellationToken)).Count == 0)
            .GreaterThan(DateTime.Now);
    }
}
