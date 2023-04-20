using System.Globalization;
using CIS.Core;
using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentArchiveService.Clients;
using DomainServices.DocumentArchiveService.Contracts;
using DomainServices.UserService.Clients;
using Google.Protobuf;
using NOBY.Api.Endpoints.Shared;

namespace NOBY.Api.Endpoints.Cases.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    private readonly IDateTime _dateTime;
    private readonly ICaseServiceClient _caseService;
    private readonly IDocumentArchiveServiceClient _documentArchiveService;
    private readonly ITempFileManager _tempFileManager;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    
    public async Task Handle(UpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var user = await _userServiceClient.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var filePaths = new List<string>();
        var documentIds = new List<string>();

        foreach (var attachment in request.Attachments)
        {
            var documentId = await _documentArchiveService.GenerateDocumentId(new GenerateDocumentIdRequest(), cancellationToken);
            
            var filePath = _tempFileManager.ComposeFilePath(attachment.DocumentInformation.Guid!.Value.ToString());
            _tempFileManager.CheckIfDocumentExist(filePath);
            
            var file = await _tempFileManager.GetDocument(filePath, cancellationToken);
            
            await _documentArchiveService.UploadDocument(new UploadDocumentRequest
            {
                BinaryData = ByteString.CopyFrom(file),
                Metadata = new DocumentMetadata
                {
                    CaseId = request.CaseId,
                    DocumentId = documentId,
                    ContractNumber = caseDetail.Data.ContractNumber,
                    CreatedOn = _dateTime.Now.Date,
                    AuthorUserLogin = user.CPM ?? user.Id.ToString(CultureInfo.InvariantCulture),
                    Description = attachment.DocumentInformation.Description ?? string.Empty,
                    EaCodeMainId = attachment.DocumentInformation.EaCodeMainId,
                    Filename = attachment.DocumentInformation.FileName,
                    FormId = attachment.FormId ?? string.Empty
                },
                NotifyStarBuild = false
            }, cancellationToken);
            
            documentIds.Add(documentId);
            filePaths.Add(filePath);
        }

        var completeTaskRequest = new CompleteTaskRequest
        {
            TaskIdSb = request.TaskIdSB,
            TaskUserResponse = request.TaskUserResponse
        };
        
        completeTaskRequest.TaskDocumentIds.AddRange(documentIds);
        
        await _caseService.CompleteTask(completeTaskRequest, cancellationToken);

        _tempFileManager.BatchDelete(filePaths);
    }

    public UpdateTaskDetailHandler(
        IDateTime dateTime,
        ICaseServiceClient caseService,
        IDocumentArchiveServiceClient documentArchiveService,
        ITempFileManager tempFileManager,
        IUserServiceClient userServiceClient,
        ICurrentUserAccessor currentUserAccessor)
    {
        _dateTime = dateTime;
        _caseService = caseService;
        _documentArchiveService = documentArchiveService;
        _tempFileManager = tempFileManager;
        _userServiceClient = userServiceClient;
        _currentUserAccessor = currentUserAccessor;
    }
}