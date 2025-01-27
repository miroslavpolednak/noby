﻿using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Services.WorkflowTask;

namespace NOBY.Api.Endpoints.Workflow.UpdateTaskDetail;

internal sealed class UpdateTaskDetailHandler(
    ICodebookServiceClient _codebookService,
    ICurrentUserAccessor _currentUserAccessor,
    Services.UploadDocumentToArchive.IUploadDocumentToArchiveService _uploadDocumentToArchive,
    ICaseServiceClient _caseService,
    SharedComponents.Storage.ITempStorage _tempFileManager,
    ISalesArrangementServiceClient _salesArrangementService,
    ILogger<UpdateTaskDetailHandler> _logger)
        : IRequestHandler<WorkflowUpdateTaskDetailRequest>
{
    public async Task Handle(WorkflowUpdateTaskDetailRequest request, CancellationToken cancellationToken)
    {
        var caseDetail = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);

        var taskIdSb = request.TaskIdSB ?? (await _caseService.GetProcessByProcessId(request.CaseId, request.TaskId, cancellationToken)).ProcessIdSb;

        var taskDetail = await _caseService.GetTaskDetail(taskIdSb, cancellationToken);

        // validace requestu
        await validateTaskUpdate(taskDetail, cancellationToken);

        var completeTaskRequest = new CompleteTaskRequest
        {
            CaseId = request.CaseId,
            TaskIdSb = taskIdSb,
            TaskResponseTypeId = request.TaskResponseTypeId,
            TaskTypeId = taskDetail.TaskObject.TaskTypeId,
            TaskUserResponse = request.TaskUserResponse,
            TaskId = request.TaskId,
            CompletionTypeId = setCompletitionType(taskDetail.TaskObject.TaskTypeId, request)
        };

        // prilohy
        var attachments = getAttachments(taskDetail, request);
        var documentIds = await getDocumentIds(taskDetail, request, caseDetail, attachments, cancellationToken);
        if (documentIds is not null)
        {
            completeTaskRequest.TaskDocumentIds.AddRange(taskDetail.TaskDetail.TaskDocumentIds.Concat(documentIds));
        }

        await _caseService.CompleteTask(completeTaskRequest, cancellationToken);

        // retence
        if (taskDetail.TaskObject.TaskTypeId == (int)WorkflowTaskTypes.RetentionRefixation)
        {
            await updateRetentionRefixationSalesArrangementParameters(request, taskDetail.TaskObject.ProcessId, cancellationToken);
        }

        if (attachments?.Any() ?? false)
        {
            await _tempFileManager.Delete(attachments.Select(t => t.TempFileId), cancellationToken);
        }
    }

    /// <summary>
    /// update RC2 atributu na retenci
    /// </summary>
    private async Task updateRetentionRefixationSalesArrangementParameters(WorkflowUpdateTaskDetailRequest request, long processId, CancellationToken cancellationToken)
    {
        // najit retencni SA
        var saList = await _salesArrangementService.GetSalesArrangementList(request.CaseId, cancellationToken);
        var saId = saList.SalesArrangements.FirstOrDefault(t => t.ProcessId == processId)?.SalesArrangementId;

        if (saId.HasValue)
        {
            // musim vytahnout cely parametrs objekt a zase ho preulozit s upravenym atributem
            var saInstance = await _salesArrangementService.GetSalesArrangement(saId.Value, cancellationToken);

            var saRequest = new DomainServices.SalesArrangementService.Contracts.UpdateSalesArrangementParametersRequest
            {
                SalesArrangementId = saInstance.SalesArrangementId
            };

            if (saInstance.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRetention)
            {
                saInstance.Retention.ManagedByRC2 = true;
                saRequest.Retention = saInstance.Retention;
            }
            else if (saInstance.SalesArrangementTypeId == (int)SalesArrangementTypes.MortgageRefixation)
            {
                saInstance.Refixation.ManagedByRC2 = true;
                saRequest.Refixation = saInstance.Refixation;
            }

            await _salesArrangementService.UpdateSalesArrangementParameters(saRequest, cancellationToken);
        }
        else
        {
            _logger.LogInformation("SalesArrangement for Case {CaseId} with ProcessId {ProcessId} not found", request.CaseId, processId);
        }
    }

    private int? setCompletitionType(int taskTypeId, WorkflowUpdateTaskDetailRequest request)
    {
        int? completionTypeId = null;

        // podepisovani
        if (taskTypeId == (int)WorkflowTaskTypes.Signing)
        {
            completionTypeId = _currentUserAccessor!.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments) ? 2 : 1;

            if (completionTypeId == 2 && !request.TaskResponseTypeId.HasValue)
            {
                throw new NobyValidationException(90032);
            }
        }

        // retence
        if (taskTypeId == (int)WorkflowTaskTypes.RetentionRefixation)
        {
            if (!_currentUserAccessor!.HasPermission(UserPermissions.WFL_TASK_DETAIL_RefinancingOtherManage))
            {
                throw new CisAuthorizationException("Missing WFL_TASK_DETAIL_RefinancingOtherManage permission");
            }
            completionTypeId = 2;
        }

        return completionTypeId;
    }

    private static List<Services.UploadDocumentToArchive.DocumentMetadata>? getAttachments(GetTaskDetailResponse taskDetail, WorkflowUpdateTaskDetailRequest request)
    {
        return request
            .Attachments?
            .Select(t => new Services.UploadDocumentToArchive.DocumentMetadata
            {
                Description = t.Description,
                EaCodeMainId = t.EaCodeMainId,
                TempFileId = t.Guid!.Value,
                FormId = taskDetail.TaskObject.TaskTypeId == 6 && !string.IsNullOrWhiteSpace(taskDetail.TaskDetail?.Signing?.FormId) ? taskDetail.TaskDetail?.Signing?.FormId : null
            })
            .ToList();
    }

    private async Task<List<string>?> getDocumentIds(GetTaskDetailResponse taskDetail, WorkflowUpdateTaskDetailRequest request, Case caseDetail, List<Services.UploadDocumentToArchive.DocumentMetadata>? attachments, CancellationToken cancellationToken)
    {
        if (attachments?.Any() ?? false)
        {
            return await _uploadDocumentToArchive.Upload(caseDetail.CaseId, caseDetail.Data?.ContractNumber, attachments, cancellationToken);
        }
        else if (taskDetail.TaskObject.TaskTypeId == (int)WorkflowTaskTypes.Signing
            && taskDetail.TaskObject.SignatureTypeId == (int)SignatureTypes.Paper
            && request.TaskResponseTypeId == 0
            && _currentUserAccessor.HasPermission(UserPermissions.WFL_TASK_DETAIL_SigningAttachments))
        {
            throw new NobyValidationException(90032);
        }
        return null;
    }

    /// <summary>
    /// Validace requestu na zacatku flow
    /// </summary>
    private async Task validateTaskUpdate(GetTaskDetailResponse taskDetail, CancellationToken cancellationToken)
    {
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
    }

    // povolene typy tasku
    private static int[] _allowedTaskTypeIds =
        [
            (int)WorkflowTaskTypes.Dozadani,
            (int)WorkflowTaskTypes.Signing,
            (int)WorkflowTaskTypes.RetentionRefixation
        ];
}