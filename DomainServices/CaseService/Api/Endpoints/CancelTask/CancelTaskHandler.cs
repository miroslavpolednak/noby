using CIS.Foms.Enums;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.SalesArrangementService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        var taskDetail = await _mediator.Send(new GetTaskDetailRequest{ TaskIdSb = request.TaskIdSB }, cancellationToken);
        await _sbWebApi.CancelTask(request.TaskIdSB, cancellationToken);
        
        // set flow switches
        if (taskDetail.TaskObject?.TaskTypeId == 2)
        {
            var saId = await _salesArrangementService.GetProductSalesArrangement(request.CaseId, cancellationToken);
            await _salesArrangementService.SetFlowSwitches(saId.SalesArrangementId, new()
            {
                new() { FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPApproved, Value = false },
                new() { FlowSwitchId = (int)FlowSwitches.IsWflTaskForIPNotApproved, Value = false }
            }, cancellationToken);
        }
        
        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly IMediator _mediator;
    private readonly ISbWebApiClient _sbWebApi;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CancelTaskHandler(
        IMediator mediator,
        ISbWebApiClient sbWebApi,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _mediator = mediator;
        _sbWebApi = sbWebApi;
        _salesArrangementService = salesArrangementService;
    }
}
