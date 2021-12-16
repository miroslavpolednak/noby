namespace DomainServices.CaseService.Api.Dto;

internal sealed class GetCaseListMediatrRequest
    : IRequest<Contracts.GetCaseListResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int UserId { get; init; }
    public int? State { get; init; }
    public CIS.Infrastructure.gRPC.CisTypes.PaginationRequest Pagination { get; init; }

    public GetCaseListMediatrRequest(Contracts.GetCaseListRequest request)
    {
        UserId = request.UserId;
        State = request.State;
        Pagination = CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.ParseOrDefault(request.Pagination);
    }
}
