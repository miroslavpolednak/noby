using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.CodebookService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.CancelTask;

internal sealed class CancelTaskHandler
    : IRequestHandler<CancelTaskRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(CancelTaskRequest request, CancellationToken cancellationToken)
    {
        // case entity
        Database.Entities.Case entity = await _dbContext.Cases.FirstOrDefaultAsync(t => t.CaseId == request.CaseId, cancellationToken)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.CaseNotFound, request.CaseId);

        var taskDetail = await _mediator.Send(new GetTaskDetailRequest{ TaskIdSb = request.TaskIdSB }, cancellationToken);
        await _sbWebApi.CancelTask(request.TaskIdSB, cancellationToken);

        // set flow switches
        var mandant = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == entity.ProductTypeId).MandantId;
        if (taskDetail.TaskObject?.TaskTypeId == 2 && entity.State == (int)CaseStates.InProgress && mandant == (int)Mandants.Kb)
        {
            var saId = (await _salesArrangementService.GetProductSalesArrangements(request.CaseId, cancellationToken)).First().SalesArrangementId;
            await _salesArrangementService.SetFlowSwitches(saId, new()
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
    private readonly ICodebookServiceClient _codebookService;
    private readonly Database.CaseServiceDbContext _dbContext;

    public CancelTaskHandler(
        IMediator mediator,
        ISbWebApiClient sbWebApi,
        ISalesArrangementServiceClient salesArrangementService,
        ICodebookServiceClient codebookService,
        Database.CaseServiceDbContext dbContext)
    {
        _mediator = mediator;
        _sbWebApi = sbWebApi;
        _salesArrangementService = salesArrangementService;
        _codebookService = codebookService;
        _dbContext = dbContext;
    }
}
