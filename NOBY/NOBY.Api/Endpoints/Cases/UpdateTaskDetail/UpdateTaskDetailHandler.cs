using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Cases.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly Infrastructure.Services.TempFileManager.ITempFileManager _tempFileManager;
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
            documentIds.AddRange(await _tempFileManager.UploadToArchive(caseDetail.CaseId, caseDetail.Data?.ContractNumber, attachments, cancellationToken));
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

        _tempFileManager.BatchDelete(attachments);
    }

    public UpdateTaskDetailHandler(

        ICaseServiceClient caseService,
        Infrastructure.Services.TempFileManager.ITempFileManager tempFileManager)
    {
        _caseService = caseService;
        _tempFileManager = tempFileManager;
    }
}