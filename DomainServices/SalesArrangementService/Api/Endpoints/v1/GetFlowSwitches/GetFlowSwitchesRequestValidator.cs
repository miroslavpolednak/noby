using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetFlowSwitches;

internal sealed class GetFlowSwitchesRequestValidator
    : AbstractValidator<Contracts.GetFlowSwitchesRequest>
{
    public GetFlowSwitchesRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);
    }
}
