using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.DocumentOnSAService.Api.Extensions;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.DocumentOnSAService.ExternalServices.SbQueues.V1.Repositories;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SetProcessingDateInSbQueues;

public class SetProcessingDateInSbQueuesHandler : IRequestHandler<SetProcessingDateInSbQueuesRequest, Empty>
{
    private readonly ISbQueuesRepository _sbQueuesRepository;
    private readonly ICaseServiceClient _caseService;
    private readonly ILogger<SetProcessingDateInSbQueuesHandler> _logger;
    private readonly TimeProvider _dateTime;

    public SetProcessingDateInSbQueuesHandler(
      ISbQueuesRepository sbQueuesRepository,
        ICaseServiceClient caseService,
        ILogger<SetProcessingDateInSbQueuesHandler> logger,
        TimeProvider dateTime)
    {

        _sbQueuesRepository = sbQueuesRepository;
        _caseService = caseService;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task<Empty> Handle(SetProcessingDateInSbQueuesRequest request, CancellationToken cancellationToken)
    {
        // If starbuild is offline, we don't want break task completion so we catch exception and log 
        try
        {
            var task = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
                            .Find(t => t.TaskTypeId == 6 && t.TaskId == request.TaskId && t.SignatureTypeId == 1);

            if (task == null)
                return new();


            var taskDetail = await _caseService.GetTaskDetail(task.TaskIdSb, cancellationToken);
            
            var signing = taskDetail.TaskDetail.AmendmentsCase switch
            {
                TaskDetailItem.AmendmentsOneofCase.Signing => taskDetail.TaskDetail.Signing,
                _ => throw ErrorCodeMapper.CreateArgumentException(ErrorCodeMapper.AmendmentHasToBeOfTypeSigning)
            };

            var currentDate = _dateTime.GetLocalNow().DateTime;

            if (signing.ProposalForEntry?.Count > 0)
            {
                foreach (var attachmentId in signing.ProposalForEntry)
                {
                    if (long.TryParse(attachmentId, out var attachmentIdNum))
                    {
                        await _sbQueuesRepository.UpdateAttachmentProcessingDate(attachmentIdNum, currentDate, cancellationToken);
                    }
                }
            }
            else if (signing.DocumentForSigningType.Equals("A", StringComparison.OrdinalIgnoreCase) && long.TryParse(signing.DocumentForSigning, out var attachmentIdNum))
            {
                await _sbQueuesRepository.UpdateAttachmentProcessingDate(attachmentIdNum, currentDate, cancellationToken);
            }
            else if (signing.DocumentForSigningType.Equals("D", StringComparison.OrdinalIgnoreCase) && long.TryParse(signing.DocumentForSigning, out var documentIdNum))
            {
                await _sbQueuesRepository.UpdateDocumentProcessingDate(documentIdNum, currentDate, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.CustomExp(ex);
        }

        return new Empty();
    }
}
