using CIS.Core;
using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.DocumentOnSAService.Api.Extensions;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using Google.Protobuf.WellKnownTypes;
using System.Globalization;
using static DomainServices.CodebookService.Contracts.v1.WorkflowTaskStatesResponse.Types.WorkflowTaskStatesItem.Types;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SetProcessingDateInSbQueues;

public class SetProcessingDateInSbQueuesHandler : IRequestHandler<SetProcessingDateInSbQueuesRequest, Empty>
{
    private readonly ISbQueuesRepository _sbQueuesRepository;
    private readonly ICaseServiceClient _caseService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<SetProcessingDateInSbQueuesHandler> _logger;
    private readonly IDateTime _dateTime;

    public SetProcessingDateInSbQueuesHandler(
      ISbQueuesRepository sbQueuesRepository,
        ICaseServiceClient caseService,
        ICodebookServiceClient codebookService,
        ILogger<SetProcessingDateInSbQueuesHandler> logger,
        IDateTime dateTime)
    {

        _sbQueuesRepository = sbQueuesRepository;
        _caseService = caseService;
        _codebookService = codebookService;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task<Empty> Handle(SetProcessingDateInSbQueuesRequest request, CancellationToken cancellationToken)
    {
        var currentDate = _dateTime.Now;

        var workflowTaskStates = await _codebookService.WorkflowTaskStates(cancellationToken);
        var nonFinalStates = workflowTaskStates.Where(s => s.Flag == EWorkflowTaskStateFlag.None).Select(s => s.Id);
        var tasksList = (await _caseService.GetTaskList(request.CaseId, cancellationToken)).Where(t => t.TaskTypeId == 6 && t.SignatureTypeId == 1);
        // Check if task is in request is here (tasksList) if not => end 
        var tasksInNonFinalState = tasksList.Where(t => nonFinalStates.Contains(t.StateIdSb));

        List<(long documentId, int taskIdSb)> taskIdSbForSpecifiedDocumentId = new();
        List<(AmendmentSigning signing, GetTaskDetailResponse taskDetail)> signingWithTaskDetail = new();

        await GetDocumentIds(tasksList, taskIdSbForSpecifiedDocumentId, signingWithTaskDetail, cancellationToken);

        var documentIdForRequestTaskId = taskIdSbForSpecifiedDocumentId.Where(t => t.taskIdSb == request.TaskIdSb).Select(s => s.documentId).FirstOrDefault();
        if (documentIdForRequestTaskId != 0)
        {
            var groupForDucumentId = taskIdSbForSpecifiedDocumentId.Where(d => d.documentId == documentIdForRequestTaskId);
            var nonFinalTaskForGroup = tasksInNonFinalState.Where(r => groupForDucumentId.Select(s => s.taskIdSb).Contains(r.TaskIdSb));

            if (!nonFinalTaskForGroup.Any()) // Indicate last completed task in group for documentId
            {
                try
                {
                    await UpdateSbQueues(currentDate, documentIdForRequestTaskId, cancellationToken);
                }
                catch (Exception exp)
                {
                    await UpdateSbQueues(null, documentIdForRequestTaskId, cancellationToken);
                    _logger.UpdateOfSbQueuesFailed(documentIdForRequestTaskId, exp);
                }
            }
        }

        return new Empty();
    }

    private async Task UpdateSbQueues(DateTime? currentDate, long documentIdForRequestTaskId, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
             _sbQueuesRepository.UpdateAttachmentProcessingDate(documentIdForRequestTaskId, currentDate, cancellationToken),
             _sbQueuesRepository.UpdateClientProcessingDate(documentIdForRequestTaskId, currentDate, cancellationToken),
             _sbQueuesRepository.UpdateDocumentProcessingDate(documentIdForRequestTaskId, currentDate, cancellationToken)
             );
    }

    private async Task GetDocumentIds(IEnumerable<WorkflowTask> tasksList, List<(long documentId, int taskIdSb)> taskIdSbForSpecifiedDocumentId, List<(AmendmentSigning signing, GetTaskDetailResponse taskDetail)> signingWithTaskDetail, CancellationToken cancellationToken)
    {
        foreach (var task in tasksList)
        {
            var taskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);

            var signing = taskDetail.TaskDetail.AmendmentsCase switch
            {
                TaskDetailItem.AmendmentsOneofCase.Signing => taskDetail.TaskDetail.Signing,
                _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AmendmentHasToBeOfTypeSigning)
            };

            signingWithTaskDetail.Add((signing, taskDetail));

            if (signing.ProposalForEntry?.Count > 0)
            {
                foreach (var attachmentId in signing.ProposalForEntry)
                {
                    var documentId = await _sbQueuesRepository.GetDocumentIdAccordingAtchId(attachmentId, cancellationToken);
                    taskIdSbForSpecifiedDocumentId.Add((documentId, task.TaskIdSb));
                }
            }
            else if (signing.DocumentForSigningType.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                var documentId = await _sbQueuesRepository.GetDocumentIdAccordingAtchId(signing.DocumentForSigning, cancellationToken);
                taskIdSbForSpecifiedDocumentId.Add((documentId, task.TaskIdSb));
            }
            else if (signing.DocumentForSigningType.Equals("D", StringComparison.OrdinalIgnoreCase))
            {
                taskIdSbForSpecifiedDocumentId.Add((long.Parse(signing.DocumentForSigning!, CultureInfo.InvariantCulture), task.TaskIdSb));
            }
            else
            {
                throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedDocumentForSigningType, signing.DocumentForSigningType);
            }
        }
    }
}
