using CIS.Infrastructure.WebApi.Validation;
using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class SearchCasesRequestValidator
    : AbstractValidator<SearchCasesRequest>
{
    public SearchCasesRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}
