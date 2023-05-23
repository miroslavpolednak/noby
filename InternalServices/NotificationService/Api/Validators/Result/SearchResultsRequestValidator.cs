using CIS.Infrastructure.CisMediatR.GrpcValidation;
using CIS.InternalServices.NotificationService.Api.Validators.Common;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Result;
using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators.Result;

public class SearchResultsRequestValidator : AbstractValidator<SearchResultsRequest>
{
    public SearchResultsRequestValidator()
    {
        RuleFor(request => request)
            .Must(request =>
                !string.IsNullOrEmpty(request.CustomId) ||
                !string.IsNullOrEmpty(request.DocumentId) ||
                !string.IsNullOrEmpty(request.Identity) ||
                !string.IsNullOrEmpty(request.IdentityScheme))
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.AtLeastOneParameterRequired)
            .Must(request =>
                (string.IsNullOrEmpty(request.Identity) && string.IsNullOrEmpty(request.IdentityScheme)) ||
                (!string.IsNullOrEmpty(request.Identity) && !string.IsNullOrEmpty(request.IdentityScheme)))
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.BothIdentityAndIdentitySchemeRequired);
        
        When(request => request.Identity is not null && request.IdentityScheme is not null , () =>
        {
            RuleFor(request => new Identifier
                {
                    Identity = request.Identity!,
                    IdentityScheme = request.IdentityScheme!
                })
                .SetValidator(new IdentifierValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentifierInvalid);
        });
        
        When(request => request.DocumentId is not null, () =>
        {
            RuleFor(request => request.DocumentId!)
                .SetValidator(new DocumentIdValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.DocumentIdInvalid);
        });
        
        When(request => request.CustomId is not null, () =>
        {
            RuleFor(request => request.CustomId!)
                .SetValidator(new CustomIdValidator())
                    .WithErrorCode(ErrorHandling.ErrorCodeMapper.CustomIdInvalid);
        });
        
    }
}