using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRetentionDocument;

internal sealed class GenerateRetentionDocumentRequestValidator : AbstractValidator<GenerateRetentionDocumentRequest>
{
    public GenerateRetentionDocumentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddDays(1).Date.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");
        
        RuleFor(t => t)
        .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
        .WithErrorCode(90056);
    }
}