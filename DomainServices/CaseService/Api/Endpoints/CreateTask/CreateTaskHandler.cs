using DomainServices.CaseService.Contracts;
using DomainServices.CaseService.ExternalServices.SbWebApi.V1;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, CreateTaskResponse>
{
    public async Task<CreateTaskResponse> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> metadata = new();
        metadata.Add(getTaskTypeKey(), request.TaskRequest);
        metadata.Add("ukol_uver_id", request.CaseId.ToString(System.Globalization.CultureInfo.InvariantCulture));
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
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            Metadata = metadata
        }, cancellationToken);

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

    private readonly ISbWebApiClient _sbWebApi;

    public CreateTaskHandler(ISbWebApiClient sbWebApi)
    {
        _sbWebApi = sbWebApi;
    }
}
