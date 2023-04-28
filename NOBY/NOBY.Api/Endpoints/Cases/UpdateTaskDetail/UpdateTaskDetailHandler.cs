using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Cases.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly Infrastructure.Services.TempFileManager.ITempFileManager _tempFileManager;
    
    public async Task Handle(UpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        List<string>? documentIds = new();
        var attachments = request.Attachments?
            .Select(t => new Infrastructure.Services.TempFileManager.TempDocumentInformation
            {
                Description = t.DocumentInformation.Description,
                EaCodeMainId = t.DocumentInformation.EaCodeMainId,
                FileName = t.DocumentInformation.FileName,
                TempGuid = t.DocumentInformation.Guid!.Value,
                FormId = t.FormId
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