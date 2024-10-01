using DomainServices.CaseService.Contracts;
using ExternalServices.SbWebApi.V1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.v1.CancelTask;

internal sealed class CancelTaskHandler(
    IMediator _mediator,
    ISbWebApiClient _sbWebApi,
    ISalesArrangementServiceClient _salesArrangementService,
    ICodebookServiceClient _codebookService,
    Database.CaseServiceDbContext _dbContext)
        : IRequestHandler<CancelTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // case entity
        Database.Entities.Case entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        var taskDetail = await _mediator.Send(new GetTaskDetailRequest
        {
            TaskIdSb = request.TaskIdSB
        }, cancellationToken);

        await _sbWebApi.CancelTask(request.TaskIdSB, cancellationToken);

        // set flow switches
        var mandant = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == entity.ProductTypeId).MandantId;
        if (taskDetail.TaskObject?.TaskTypeId == (int)WorkflowTaskTypes.PriceException 
            && CaseHelpers.IsCaseInState([EnumCaseStates.InProgress], (EnumCaseStates)entity.State) 
            && mandant == (int)Mandants.Kb)
        {
            var saId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First().SalesArrangementId;
            await _salesArrangementService.SetFlowSwitches(saId,
            [
                new() { FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPApproved, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPNotApproved, Value = false }
            ], cancellationToken);
        }

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
}
