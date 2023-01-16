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
                .WithErrorCode(ErrorCodes.SearchResult.AtLeastOneParameterRequired)
                .WithMessage($"{nameof(SearchResultsRequest)} must contain at least 1 non-empty search parameter.")
            .Must(request =>
                (string.IsNullOrEmpty(request.Identity) && string.IsNullOrEmpty(request.IdentityScheme)) ||
                (!string.IsNullOrEmpty(request.Identity) && !string.IsNullOrEmpty(request.IdentityScheme)))
                .WithErrorCode(ErrorCodes.SearchResult.IdentityInvalid)
                .WithMessage($"{nameof(SearchResultsRequest)} must contain either both {nameof(SearchResultsRequest.Identity)} and {nameof(SearchResultsRequest.IdentityScheme)} or none.");
    }
}