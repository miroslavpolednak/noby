using FluentValidation;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomersOnSa;

internal sealed class GetCustomersOnSaValidator
    : AbstractValidator<GetCustomersOnSaRequest>
{
    public GetCustomersOnSaValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}