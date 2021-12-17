namespace FOMS.Api.Endpoints.Case.Dto;

internal class GetCaseRequest
    : IRequest<Dto.GetCaseResponse>
{
    public long CaseId { get; set; }

    public GetCaseRequest(long caseId)
    {
        CaseId = caseId;
    }
}
