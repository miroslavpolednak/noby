using CIS.Core.Security;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Workflow.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler : IRequestHandler<UpdateTaskDetailRequest>
{
    public async Task Handle(UpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        var taskDetail = await _caseService.GetTaskDetail(request.TaskIdSB, cancellationToken);

        var inactiveStates = (await _codebookService.WorkflowTaskStates(cancellationToken))
            .Where(t => t.Flag.HasFlag(DomainServices.CodebookService.Contracts.v1.WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types.EWorkflowTaskStateFlag.Inactive))
            .Select(t => t.Id)
            .ToArray();

        if (taskDetail.TaskObject.Cancelled
            || inactiveStates.Contains(taskDetail.TaskObject.StateIdSb))
        {
            throw new NobyValidationException(90032, "Inactive task");
        }

        if (!_allowedTaskTypeIds.Contains(taskDetail.TaskObject.TaskTypeId))
        {
            throw new NobyValidationException(90032, "TaskTypeId not allowed");
        }

        WorkflowHelpers.ValidateTaskManagePermission(
            taskDetail.TaskObject!.TaskTypeId,
            taskDetail.TaskObject.SignatureTypeId,
            taskDetail.TaskObject.PhaseTypeId,
            taskDetail.TaskObject.ProcessTypeId,
            _currentUserAccessor);

        List<string>? documentIds = new();
        var attachments = request
            .Attachments?
            .Select(t => new Services.UploadDocumentToArchive.DocumentMetadata
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                TempFileId = t.Guid!.Value,
                FormId = request.TaskTypeId == 6 ? taskDetail.TaskDetail?.Signing?.FormId : null
            })
            .ToList();

        if (attachments?.Any() ?? false)
        {
            documentIds.AddRange(await _uploadDocumentToArchive.Upload(caseDetail.CaseId, caseDetail.Data?.ContractNumber, attachments, cancellationToken));
        }
        else if (taskDetail.TaskObject.TaskTypeId == 6
            && taskDetail.TaskObject.SignatureTypeId == (int)SignatureTypes.Paper
            && request.TaskResponseTypeId == 0
            && _currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments))
        {
            throw new NobyValidationException(90032);
        }

        var completeTaskRequest = new CompleteTaskRequest
        {
            CaseId = request.CaseId,
            TaskIdSb = request.TaskIdSB,
            TaskResponseTypeId = request.TaskResponseTypeId,
            TaskTypeId = request.TaskTypeId,
            TaskUserResponse = request.TaskUserResponse,
            TaskId = request.TaskId
        };

        completeTaskRequest.TaskDocumentIds.AddRange(taskDetail.TaskDetail.TaskDocumentIds.Concat(documentIds));

        // podepisovani
        if (request.TaskTypeId == 6)
        {
            completeTaskRequest.CompletionTypeId = _currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments) ? 2 : 1;

            if (completeTaskRequest.CompletionTypeId == 2 && !request.TaskResponseTypeId.HasValue)
            {
                throw new NobyValidationException(90032);
            }
        }

        // retence
        if (request.TaskTypeId == 9)
        {
            if (!_currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            {
                throw new CisAuthorizationException("Missing WFL_TASK_DETAIL_RefinancingOtherManage permission");
            }

            completeTaskRequest.CompletionTypeId = 2;
        }

        await _caseService.CompleteTask(completeTaskRequest, cancellationToken);

        // retence
        if (request.TaskTypeId == 9)
        {
            var saRequest = new DomainServices.SalesArrangementService.Contracts.UpdateSalesArrangementParametersRequest
            {
                SalesArrangementId = 1
            };
            await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
        }

        if (attachments?.Any() ?? false)
        {
            await _tempFileManager.Delete(attachments.Select(t => t.TempFileId), cancellationToken);
        }
    }

    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ICaseServiceClient _caseService;
    private readonly SharedComponents.Storage.ITempStorage _tempFileManager;
    private readonly Services.UploadDocumentToArchive.IUploadDocumentToArchiveService _uploadDocumentToArchive;
    
    private static int[] _allowedTaskTypeIds = [ 1, 6, 9 ];

    public UpdateTaskDetailHandler(
        ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUserAccessor,
        Services.UploadDocumentToArchive.IUploadDocumentToArchiveService uploadDocumentToArchive,
        ICaseServiceClient caseService,
        SharedComponents.Storage.ITempStorage tempFileManager,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _codebookService = codebookService;
        _currentUserAccessor = currentUserAccessor;
        _uploadDocumentToArchive = uploadDocumentToArchive;
        _caseService = caseService;
        _tempFileManager = tempFileManager;
        _salesArrangementService = salesArrangementService;
    }
}