namespace NOBY.Api.Endpoints.Workflow.GetConsultationTypes;

internal sealed record GetConsultationTypesRequest(long CaseId, long ProcessId)
    : IRequest<List<GetConsultationTypesResponseItem>>
{
}
