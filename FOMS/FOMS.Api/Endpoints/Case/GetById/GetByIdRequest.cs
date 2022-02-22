namespace FOMS.Api.Endpoints.Case.GetById;

internal record GetByIdRequest(long CaseId)
    : IRequest<Dto.CaseModel>
{
}
