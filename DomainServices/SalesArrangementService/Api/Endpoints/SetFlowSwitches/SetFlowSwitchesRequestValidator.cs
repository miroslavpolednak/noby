using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SetFlowSwitches;

internal sealed class SetFlowSwitchesRequestValidator
    : AbstractValidator<Contracts.SetFlowSwitchesRequest>
{
    public SetFlowSwitchesRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("18010");

        RuleFor(t => t.FlowSwitches)
            .NotNull()
            .WithMessage("FlowSwitches collection must not be empty").WithErrorCode("0");
    }
}
