using FluentValidation;
using NOBY.Api.Endpoints.DocumentOnSA.Search;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseValidator : AbstractValidator<SearchDocumentsOnSaOnCaseRequest>
{
    public SearchDocumentsOnSaOnCaseValidator()
    {
        RuleFor(e => e.EACodeMainId).NotNull().WithMessage($"{nameof(SearchDocumentsOnSaRequest.EACodeMainId)} is required");
    }
}
