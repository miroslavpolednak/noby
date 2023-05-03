using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Cases.CreateTask;

internal sealed class CreateTaskHandler
    : IRequestHandler<CreateTaskRequest, long>
{
    public async Task<long> Handle(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        // kontrola existence Case
        var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        List<string>? documentIds = new();
        var attachments = request.Attachments?
            .Select(t => new Infrastructure.Services.TempFileManager.TempDocumentInformation
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                FileName = t.FileName,
                TempGuid = t.Guid!.Value
            })
            .ToList();

        if (attachments?.Any() ?? false)
        {
            documentIds.AddRange(await _tempFileManager.UploadToArchive(caseInstance.CaseId, caseInstance.Data?.ContractNumber, attachments, cancellationToken));
        }

        var dsRequest = new DomainServices.CaseService.Contracts.CreateTaskRequest
        {
            CaseId = caseInstance.CaseId,
            ProcessId = request.ProcessId,
            TaskTypeId = request.TaskTypeId,
            TaskRequest = request.TaskUserRequest ?? "",
            TaskSubtypeId = request.TaskSubtypeId
        };
        // pokud existuji nahrane prilohy
        if (documentIds.Any())
        {
            dsRequest.TaskDocumentsId.AddRange(documentIds);
        }

        var result = await _caseService.CreateTask(dsRequest, cancellationToken);

        // smazat prilohy z tempu
        _tempFileManager.BatchDelete(attachments);

        return result.TaskId;
    }

    private readonly ICaseServiceClient _caseService;
    private readonly Infrastructure.Services.TempFileManager.ITempFileManager _tempFileManager;

    public CreateTaskHandler(ICaseServiceClient caseService, Infrastructure.Services.TempFileManager.ITempFileManager tempFileManager)
    {
        _caseService = caseService;
        _tempFileManager = tempFileManager;
    }
}
