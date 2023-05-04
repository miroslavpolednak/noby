using CIS.Infrastructure.WebApi.Validation;
using FluentValidation;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal sealed class SearchCustomersRequestValidator
    : AbstractValidator<SearchCustomersRequest>
{
    public SearchCustomersRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}