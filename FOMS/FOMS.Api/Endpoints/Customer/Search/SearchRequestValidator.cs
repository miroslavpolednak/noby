using CIS.Infrastructure.WebApi.Validation;
using FluentValidation;

namespace FOMS.Api.Endpoints.Customer.Search;

internal class SearchRequestValidator
    : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}