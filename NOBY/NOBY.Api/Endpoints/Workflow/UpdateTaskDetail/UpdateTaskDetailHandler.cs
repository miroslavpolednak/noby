using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Workflow.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly Infrastructure.Services.TempFileManager.ITempFileManagerService _tempFileManager;
    private readonly Infrastructure.Services.UploadDocumentToArchive.IUploadDocumentToArchiveService _uploadDocumentToArchive;
    private static int[] _allowedTaskTypeIds = { 1, 6 };

    public async Task Handle(UpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var taskDetail = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);

        if (!_allowedTaskTypeIds.Contains(taskDetail.TaskObject.TaskTypeId))
        {
            throw new CisAuthorizationException();
        }

        List<string>? documentIds = new();
        var attachments = request.Attachments?
            .Select(t => new Infrastructure.Services.UploadDocumentToArchive.DocumentMetadata
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                FileName = t.FileName,
                TempFileId = t.Guid!.Value
            })
            .ToList();

        if (attachments?.Any() ?? false)
        {
            documentIds.AddRange(await _uploadDocumentToArchive.Upload(caseDetail.CaseId, caseDetail.Data?.ContractNumber, attachments, cancellationToken));
        }

        var completeTaskRequest = new CompleteTaskRequest
        {
            CaseId = request.CaseId,
            TaskIdSb = request.TaskIdSB,
            TaskResponseTypeId = request.TaskResponseTypeId,
            TaskTypeId = request.TaskTypeId,
            TaskUserResponse = request.TaskUserResponse
        };

        completeTaskRequest.TaskDocumentIds.AddRange(documentIds);

        await _caseService.CompleteTask(completeTaskRequest, cancellationToken);

        if (attachments?.Any() ?? false)
        {
            await _tempFileManager.Delete(attachments.Select(t => t.TempFileId), cancellationToken);
        }
    }

    public UpdateTaskDetailHandler(
        Infrastructure.Services.UploadDocumentToArchive.IUploadDocumentToArchiveService uploadDocumentToArchive,
        ICaseServiceClient caseService,
        Infrastructure.Services.TempFileManager.ITempFileManagerService tempFileManager)
    {
        _uploadDocumentToArchive = uploadDocumentToArchive;
        _caseService = caseService;
        _tempFileManager = tempFileManager;
    }
}