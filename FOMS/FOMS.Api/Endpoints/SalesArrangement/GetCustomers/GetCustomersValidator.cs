using FluentValidation;

namespace FOMS.Api.Endpoints.SalesArrangement.Validators;

internal class GetCustomersValidator
    : AbstractValidator<Dto.GetCustomersRequest>
{
    public GetCustomersValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}