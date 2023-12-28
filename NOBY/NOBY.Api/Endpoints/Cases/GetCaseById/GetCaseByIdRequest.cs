namespace NOBY.Api.Endpoints.Cases.GetCaseById;

internal sealed record GetCaseByIdRequest(long CaseId)
    : IRequest<SharedDto.CaseModel>
{
}
