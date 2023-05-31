using CIS.Foms.Enums;
using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> metadata = new();
        metadata.Add(getTaskTypeKey(), request.TaskRequest);
        metadata.Add("ukol_uver_id", request.CaseId.ToString(CultureInfo.InvariantCulture));
        metadata.Add("ukol_mandant", "2");

        // subtype
        if (request.TaskTypeId == 3)
        {
            metadata.Add("ukol_konzultace_oblast", $"{request.TaskSubtypeId}");
        }

        // ID dokumentu
        if (request.TaskDocumentsId?.Any() ?? false)
        {
            metadata.Add("wfl_refobj_dokumenty", string.Join(",", request.TaskDocumentsId) + ",");
        }

        var result = await _sbWebApi.CreateTask(new ExternalServices.SbWebApi.Dto.CreateTask.CreateTaskRequest
        {
            ProcessId = Convert.ToInt32(request.ProcessId),//IT anal neni schopna rict co s tim
            TaskTypeId = request.TaskTypeId,
            Metadata = metadata
        }, cancellationToken);

        // nastavit flow switche
        await setFlowSwitches(request, cancellationToken);

        return new CreateTaskResponse
        {
            TaskIdSB = result.TaskIdSB,
            TaskId = result.TaskId
        };

        string getTaskTypeKey() => request.TaskTypeId switch
        {
            7 => "ukol_predanihs_pozadavek",
            3 => "ukol_konzultace_pozadavek",
            _ => throw new NotImplementedException($"TaskTypeId {request.TaskTypeId} is not supported")
        };
    }

    private async Task setFlowSwitches(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        if (request.TaskTypeId == 2)
        {
            var saTypes = (await _codebookService.SalesArrangementTypes(cancellationToken))
                .Where(t => t.SalesArrangementCategory == 1)
                .Select(t => t.Id)
                .ToList();

            var saInstance = (await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken))
                .SalesArrangements
                .Where(t => saTypes.Contains(t.SalesArrangementTypeId))
                .FirstOrDefault();

            if (saInstance is not null)
            {
                await _salesArrangementService.SetFlowSwitches(saInstance.SalesArrangementId, new()
                {
                    new() { FlowSwitchId = (int)FlowSwitches.DoesWflTaskForIPExist, Value = false }
                }, cancellationToken);
            }
        }
    }

    private readonly ICodebookServiceClient _codebookService;
    private readonly ISbWebApiClient _sbWebApi;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public CreateTaskHandler(ISbWebApiClient sbWebApi, ISalesArrangementServiceClient salesArrangementService, ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _sbWebApi = sbWebApi;
    }
}
