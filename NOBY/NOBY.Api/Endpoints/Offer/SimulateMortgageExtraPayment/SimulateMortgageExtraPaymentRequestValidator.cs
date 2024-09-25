using DomainServices.CodebookService.Clients;
using FluentValidation;
using Microsoft.FeatureManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentRequestValidator
    : AbstractValidator<OfferSimulateMortgageExtraPaymentRequest>
{
    public SimulateMortgageExtraPaymentRequestValidator(ICodebookServiceClient codebookService, IFeatureManager featureManager)
    {
        When(t => !t.IsExtraPaymentFullyRepaid, () => RuleFor(t => t.ExtraPaymentAmount).Cascade(CascadeMode.Stop).NotEmpty().GreaterThan(0));

        RuleFor(t => t.ExtraPaymentReasonId)
            .MustAsync(async (id, cancellationToken) => (await codebookService.ExtraPaymentReasons(cancellationToken)).Any(t => t.Id == id));

        RuleFor(t => t.ExtraPaymentDate)
            .MustAsync(async (d, cancellationToken) => (await codebookService.GetNonBankingDays(d, d, cancellationToken)).Count == 0)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now.Date));

        RuleFor(t => t)
            .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
        .WithErrorCode(90058);

        //Check if integer
        RuleFor(t => t.ExtraPaymentAmount).Must(epAmount => epAmount is null || epAmount % 1 == 0).WithErrorCode(90073);
        RuleFor(t => t.FeeAmountDiscount).Must(feeAmount => feeAmount is null || feeAmount % 1 == 0).WithErrorCode(90073);
    }
}
