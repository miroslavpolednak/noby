using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal sealed class GenerateExtraPaymentDocumentRequestValidator : AbstractValidator<GenerateExtraPaymentDocumentRequest>
{
    public GenerateExtraPaymentDocumentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");

        RuleFor(t => t)
          .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
          .WithErrorCode(90056);
    }
}