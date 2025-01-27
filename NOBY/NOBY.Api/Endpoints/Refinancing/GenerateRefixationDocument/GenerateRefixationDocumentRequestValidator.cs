﻿using FluentValidation;
using Microsoft.FeatureManagement;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefixationDocument;

internal sealed class GenerateRefixationDocumentRequestValidator 
    : AbstractValidator<RefinancingGenerateRefixationDocumentRequest>
{
    public GenerateRefixationDocumentRequestValidator(IFeatureManager featureManager)
    {
        When(x => x.SignatureDeadline is not null,
             () =>
             {
                 RuleFor(x => x.SignatureDeadline)
                     .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1).Date))
                     .WithErrorCode(90032)
                     .WithMessage("SignatureDeadline is lower than current time");
             });
        
        RuleFor(t => t)
         .MustAsync(async (_, _) => await featureManager.IsEnabledAsync(SharedTypes.FeatureFlagsConstants.Refixation))
         .WithErrorCode(90057);
    }
}