namespace DomainServices.CaseService.Api.Dto;

internal sealed class SearchCasesMediatrRequest
    : IRequest<Contracts.SearchCasesResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int CaseOwnerUserId { get; init; }
    public int? State { get; init; }
    public string? SearchTerm { get; init; }
    public CIS.Infrastructure.gRPC.CisTypes.PaginationRequest Pagination { get; init; }

    public SearchCasesMediatrRequest(Contracts.SearchCasesRequest request)
    {
        SearchTerm = request.SearchTerm;
        CaseOwnerUserId = request.CaseOwnerUserId;
        State = request.State;
        Pagination = CIS.Infrastructure.gRPC.CisTypes.PaginationRequest.ParseOrDefault(request.Pagination);
    }
}
