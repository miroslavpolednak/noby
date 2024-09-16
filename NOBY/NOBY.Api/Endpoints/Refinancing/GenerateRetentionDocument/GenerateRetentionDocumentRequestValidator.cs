using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRetentionDocument;

internal sealed class GenerateRetentionDocumentRequestValidator 
    : AbstractValidator<RefinancingGenerateRetentionDocumentRequest>
{
    public GenerateRetentionDocumentRequestValidator(IFeatureManager featureManager)
    {
        When(x => x.SignatureDeadline is not null,
             () =>
             {
                 RuleFor(x => x.SignatureDeadline)
                     .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)))
                     .WithErrorCode(90032)
                     .WithMessage("SignatureDeadline is lower than current time");
             });
        
        RuleFor(t => t)
        .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Retention))
        .WithErrorCode(90056);
    }
}