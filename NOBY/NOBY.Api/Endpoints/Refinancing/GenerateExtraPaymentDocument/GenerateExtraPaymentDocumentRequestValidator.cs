using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

internal sealed class GenerateExtraPaymentDocumentRequestValidator : AbstractValidator<GenerateExtraPaymentDocumentRequest>
{
    public GenerateExtraPaymentDocumentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(t => t)
          .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.ExtraPayment))
          .WithErrorCode(90058);

        RuleFor(r => r.ClientKbId).NotNull().WithErrorCode(90032);
    }
}