using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;

namespace NOBY.Api.Endpoints.Cases.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public async Task Handle(UpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        
        var taskDetail = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);
        
        foreach (var attachment in request.Attachments)
        {
            var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest
            {
                EnvironmentName = EnvironmentNames.Dev
            }, cancellationToken);
            await _documentArchiveService.UploadDocument(new UploadDocumentRequest
            {
                Metadata = new DocumentMetadata
                {
                    CaseId = request.CaseId,
                    ContractNumber = caseDetail.Data.ContractNumber,
                    Completeness = 0,
                    Description = attachment.Description,
                    Filename = attachment.FileName,
                    Priority = "",
                    Status = "",
                    CreatedOn = DateTime.Now,
                    DocumentDirection = "",
                    DocumentId = documentId,
                    FolderDocument = "",
                    FormId = "",
                    MinorCodes = {  },
                    OrderId = 0,
                    SourceSystem = "",
                    AuthorUserLogin = "",
                    FolderDocumentId = "",
                    PledgeAgreementNumber = "",
                    EaCodeMainId = attachment.EaCodeMainId
                },
                NotifyStarBuild = false
            }, cancellationToken);
        }

        await _caseService.CompleteTask(new CompleteTaskRequest
        {
            TaskIdSb = request.TaskIdSB,
            TaskUserResponse = request.TaskUserResponse,
            TaskDocumentIds = {  }
        }, cancellationToken);

    }

    public UpdateTaskDetailHandler(
        ICaseServiceClient caseService,
        IDocumentArchiveServiceClient documentArchiveService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _currentUserAccessor = currentUserAccessor;
    }
}