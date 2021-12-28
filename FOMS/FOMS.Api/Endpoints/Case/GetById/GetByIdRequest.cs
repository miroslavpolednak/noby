namespace FOMS.Api.Endpoints.Case.Dto;

internal class GetByIdRequest
    : IRequest<Dto.CaseModel>
{
    public long CaseId { get; set; }

    public GetByIdRequest(long caseId)
    {
        CaseId = caseId;
    }
}
