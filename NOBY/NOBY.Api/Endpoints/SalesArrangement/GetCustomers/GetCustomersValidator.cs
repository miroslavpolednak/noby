using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomers;

internal class GetCustomersValidator
    : AbstractValidator<GetCustomersRequest>
{
    public GetCustomersValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}