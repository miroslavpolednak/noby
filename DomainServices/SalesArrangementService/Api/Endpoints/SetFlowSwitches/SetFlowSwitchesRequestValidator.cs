using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SetFlowSwitches;

internal sealed class SetFlowSwitchesRequestValidator
    : AbstractValidator<Contracts.SetFlowSwitchesRequest>
{
    public SetFlowSwitchesRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.FlowSwitches)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.FlowSwitchesIsEmpty);
    }
}
