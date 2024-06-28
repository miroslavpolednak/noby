using DomainServices.CaseService.Clients.v1;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Workflow.GetConsultationTypes;

internal sealed class GetConsultationTypesHandler(
    ICaseServiceClient _caseService, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<GetConsultationTypesRequest, List<WorkflowGetConsultationTypesResponseItem>>
{
    public async Task<List<WorkflowGetConsultationTypesResponseItem>> Handle(GetConsultationTypesRequest request, CancellationToken cancellationToken)
    {
        var processesList = (await _caseService.GetProcessList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        var matrix = await _codebookService.WorkflowConsultationMatrix(cancellationToken);

        return matrix
            .Where(t => t.IsValidFor.Any(x => processesList.Any(p => x.ProcessPhaseId == p.ProcessPhaseId && x.ProcessTypeId == p.ProcessTypeId)))
            .Select(t => new WorkflowGetConsultationTypesResponseItem
            {
                TaskSubtypeId = t.TaskSubtypeId,
                TaskSubtypeName = t.TaskSubtypeName
            }).ToList();
    }
}
