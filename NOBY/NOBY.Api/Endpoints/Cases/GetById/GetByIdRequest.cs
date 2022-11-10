namespace NOBY.Api.Endpoints.Cases.GetById;

internal record GetByIdRequest(long CaseId)
    : IRequest<Dto.CaseModel>
{
}
