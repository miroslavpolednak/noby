using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Endpoints.Common;
using CIS.InternalServices.NotificationService.LegacyContracts.Common;
using CIS.InternalServices.NotificationService.LegacyContracts.Result;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Result.Validators;

public class SearchResultsRequestValidator : AbstractValidator<SearchResultsRequest>
{
    public SearchResultsRequestValidator()
    {
        RuleFor(request => request)
            .Must(request =>
                request.CaseId.HasValue ||
                !string.IsNullOrEmpty(request.CustomId) ||
                !string.IsNullOrEmpty(request.DocumentId) ||
                !string.IsNullOrEmpty(request.Identity) ||
                !string.IsNullOrEmpty(request.IdentityScheme))
                .WithErrorCode(ErrorCodeMapper.AtLeastOneParameterRequired)
            .Must(request =>
                (string.IsNullOrEmpty(request.Identity) && string.IsNullOrEmpty(request.IdentityScheme)) ||
                (!string.IsNullOrEmpty(request.Identity) && !string.IsNullOrEmpty(request.IdentityScheme)))
                .WithErrorCode(ErrorCodeMapper.BothIdentityAndIdentitySchemeRequired);
        
        When(request => request.Identity is not null && request.IdentityScheme is not null , () =>
        {
            RuleFor(request => new Identifier
                {
                    Identity = request.Identity!,
                    IdentityScheme = request.IdentityScheme!
                })
                .SetValidator(new IdentifierValidator())
                    .WithErrorCode(ErrorCodeMapper.IdentifierInvalid);
        });
        
        When(request => request.CaseId.HasValue, () =>
        {
            RuleFor(request => request.CaseId!.Value)
                .GreaterThan(0)
                    .WithErrorCode(ErrorCodeMapper.CaseIdInvalid);
        });
        
        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                    .WithErrorCode(ErrorCodeMapper.CustomIdInvalid);
        });
        
        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                    .WithErrorCode(ErrorCodeMapper.DocumentIdInvalid);
        });
    }
}