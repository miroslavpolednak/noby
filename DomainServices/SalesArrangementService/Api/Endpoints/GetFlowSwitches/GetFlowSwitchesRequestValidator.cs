using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetFlowSwitches;

internal sealed class GetFlowSwitchesRequestValidator
    : AbstractValidator<Contracts.GetFlowSwitchesRequest>
{
    public GetFlowSwitchesRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("18010");
    }
}
