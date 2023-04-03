namespace NOBY.Api.Endpoints.Cases.GetById;

internal sealed record GetByIdRequest(long CaseId)
    : IRequest<Dto.CaseModel>
{
}
