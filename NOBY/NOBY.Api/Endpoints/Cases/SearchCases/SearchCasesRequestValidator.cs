using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class SearchCasesRequestValidator
    : AbstractValidator<CasesSearchCasesRequest>
{
    public SearchCasesRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new NOBY.Api.Validators.PaginationRequestValidator());
    }
}
