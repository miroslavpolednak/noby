using CIS.Infrastructure.CisMediatR.GrpcValidation;
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
                .WithErrorCode(ErrorHandling.ErrorCodeMapper.IdentityInvalid);
    }
}