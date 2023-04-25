namespace NOBY.Api.Endpoints.Cases.GetConsultationTypes;

internal sealed record GetConsultationTypesRequest(long CaseId, long ProcessId)
    : IRequest<List<GetConsultationTypesResponseItem>>
{
}
