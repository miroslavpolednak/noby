using DomainServices.CaseService.Clients.v1;
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
    private readonly TimeProvider _dateTime;

    public SetProcessingDateInSbQueuesHandler(
      ISbQueuesRepository sbQueuesRepository,
        ICaseServiceClient caseService,
        ICodebookServiceClient codebookService,
        ILogger<SetProcessingDateInSbQueuesHandler> logger,
        TimeProvider dateTime)
    {

        _sbQueuesRepository = sbQueuesRepository;
        _caseService = caseService;
        _codebookService = codebookService;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task<Empty> Handle(SetProcessingDateInSbQueuesRequest request, CancellationToken cancellationToken)
    {
        // If starbuild is offline, we don't want break task completion so we catch exception and logg   
        try
        {
            var tasksList = (await _caseService.GetTaskList(request.CaseId, cancellationToken)).Where(t => t.TaskTypeId == 6)
                            .ToList();

            if (!tasksList.Any(t => t.TaskId == request.TaskId && t.SignatureTypeId == 1)) { return new Empty(); }

            var workflowTaskStates = await _codebookService.WorkflowTaskStates(cancellationToken);
            var nonFinalStates = workflowTaskStates.Where(s => s.Flag == EWorkflowTaskStateFlag.None).Select(s => s.Id);
            var tasksInNonFinalState = tasksList.Where(t => nonFinalStates.Contains(t.StateIdSb));

            List<(long documentId, long taskId)> taskIdForSpecifiedDocumentId = await GetDocumentIds(tasksList, cancellationToken);

            var documentIdForRequestTaskId = taskIdForSpecifiedDocumentId.Where(t => t.taskId == request.TaskId).Select(s => s.documentId).FirstOrDefault();
            if (documentIdForRequestTaskId != 0)
            {
                var groupForDucumentId = taskIdForSpecifiedDocumentId.Where(d => d.documentId == documentIdForRequestTaskId);
                var nonFinalTaskForGroup = tasksInNonFinalState.Where(r => groupForDucumentId.Select(s => s.taskId).Contains(r.TaskId));

                if (!nonFinalTaskForGroup.Any()) // Indicate last completed task in group for documentId
                {
                    try
                    {
                        await UpdateSbQueues(_dateTime.GetLocalNow().DateTime, documentIdForRequestTaskId, cancellationToken);
                    }
                    catch (Exception exp)
                    {
                        await UpdateSbQueues(null, documentIdForRequestTaskId, cancellationToken);
                        _logger.UpdateOfSbQueuesFailed(documentIdForRequestTaskId, exp);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.CustomExp(ex);
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

    private async Task<List<(long documentId, long taskId)>> GetDocumentIds(IEnumerable<WorkflowTask> tasksList, CancellationToken cancellationToken)
    {
        List<(long documentId, long taskId)> taskIdForSpecifiedDocumentId = [];
        foreach (var task in tasksList)
        {
            var taskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);

            var signing = taskDetail.TaskDetail.AmendmentsCase switch
            {
                TaskDetailItem.AmendmentsOneofCase.Signing => taskDetail.TaskDetail.Signing,
                _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AmendmentHasToBeOfTypeSigning)
            };

            if (signing.ProposalForEntry?.Count > 0)
            {
                foreach (var attachmentId in signing.ProposalForEntry)
                {
                    var documentId = await _sbQueuesRepository.GetDocumentIdAccordingAtchId(attachmentId, cancellationToken);
                    taskIdForSpecifiedDocumentId.Add((documentId, task.TaskId));
                }
            }
            else if (signing.DocumentForSigningType.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                var documentId = await _sbQueuesRepository.GetDocumentIdAccordingAtchId(signing.DocumentForSigning, cancellationToken);
                taskIdForSpecifiedDocumentId.Add((documentId, task.TaskId));
            }
            else if (signing.DocumentForSigningType.Equals("D", StringComparison.OrdinalIgnoreCase))
            {
                taskIdForSpecifiedDocumentId.Add((long.Parse(signing.DocumentForSigning!, CultureInfo.InvariantCulture), task.TaskId));
            }
            else
            {
                throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.UnsupportedDocumentForSigningType, signing.DocumentForSigningType);
            }
        }

        return taskIdForSpecifiedDocumentId;
    }
}
