using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateSalesArrangementState;

internal sealed class UpdateSalesArrangementStateRequestValidator
    : AbstractValidator<Contracts.UpdateSalesArrangementStateRequest>
{
    public UpdateSalesArrangementStateRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithMessage("SalesArrangement State must be > 0").WithErrorCode("18079");
    }
}
