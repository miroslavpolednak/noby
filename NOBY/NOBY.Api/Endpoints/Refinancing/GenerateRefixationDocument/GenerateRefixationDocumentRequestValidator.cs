using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefixationDocument;

internal sealed class GenerateRefixationDocumentRequestValidator : AbstractValidator<GenerateRefixationDocumentRequest>
{
    public GenerateRefixationDocumentRequestValidator(IFeatureManager featureManager)
    {
        RuleFor(x => x.SignatureDeadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.ToLocalTime())
            .WithErrorCode(90032)
            .WithMessage("SignatureDeadline is lower than current time");
        
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
         .WithErrorCode(90055);
    }
}