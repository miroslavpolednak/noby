using CIS.Infrastructure.WebApi.Validation;
using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.Search;

internal class SearchRequestValidator
    : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}
