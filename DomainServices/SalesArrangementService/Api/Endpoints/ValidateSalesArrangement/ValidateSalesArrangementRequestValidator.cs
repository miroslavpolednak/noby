using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement;

internal class ValidateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.ValidateSalesArrangementRequest>
{
    public ValidateSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");
    }
}
