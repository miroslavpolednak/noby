using DomainServices.CaseService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetConsultationTypes;

internal sealed class GetConsultationTypesHandler
    : IRequestHandler<GetConsultationTypesRequest, List<GetConsultationTypesResponseItem>>
{
    public async Task<List<GetConsultationTypesResponseItem>> Handle(GetConsultationTypesRequest request, CancellationToken cancellationToken)
    {
        var processesList = (await _caseService.GetProcessList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        return processesList.Select(t => new GetConsultationTypesResponseItem
        {
            TaskSubtypeId = 1,
            taskSubtypeName = ""
        }).ToList();
    }

    private readonly ICaseServiceClient _caseService;

    public GetConsultationTypesHandler(ICaseServiceClient caseService)
    {
        _caseService = caseService;
    }
}
