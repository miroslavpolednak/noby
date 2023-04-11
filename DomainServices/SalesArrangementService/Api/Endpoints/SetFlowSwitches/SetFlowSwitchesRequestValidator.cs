using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SetFlowSwitches;

internal sealed class SetFlowSwitchesRequestValidator
    : AbstractValidator<Contracts.SetFlowSwitchesRequest>
{
    public SetFlowSwitchesRequestValidator(Database.SalesArrangementServiceDbContext dbContext)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.FlowSwitches)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.FlowSwitchesIsEmpty);

        RuleFor(t => t.SalesArrangementId)
            .Must(saId => dbContext.SalesArrangements.Any(t => t.SalesArrangementId == saId))
            .WithErrorCode(ErrorCodeMapper.SalesArrangementNotFound);
    }
}
