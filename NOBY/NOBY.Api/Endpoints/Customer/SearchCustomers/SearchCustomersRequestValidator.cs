using FluentValidation;
using NOBY.Api.Validators;

namespace NOBY.Api.Endpoints.Customer.SearchCustomers;

internal sealed class SearchCustomersRequestValidator
    : AbstractValidator<CustomerSearchCustomersRequest>
{
    public SearchCustomersRequestValidator()
    {
        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}