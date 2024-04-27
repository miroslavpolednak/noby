using CIS.Core.Security;
using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
internal sealed class SearchCasesHandler(
    ICurrentUserAccessor _userAccessor,
    CasesModelConverter _converter,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService)
        : IRequestHandler<SearchCasesRequest, SearchCasesResponse>
{
    public async Task<SearchCasesResponse> Handle(SearchCasesRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        var filterStates = getStatesFilter(request.FilterId);

        if (filterStates is not null && !_userAccessor.HasPermission(UserPermissions.CASE_ViewAfterDrawing)) 
            filterStates.Remove((int)CaseStates.InAdministration);

        DomainServices.CaseService.Contracts.SearchCasesResponse result;

        if (filterStates?.Any() ?? true)
            result = await _caseService.SearchCases(paginable, _userAccessor.User!.Id, getStatesFilter(request.FilterId), request.Term, cancellationToken);
        else
            result = new DomainServices.CaseService.Contracts.SearchCasesResponse { Pagination = new() };

            // transform
        return new SearchCasesResponse
        {
            Rows = await _converter.FromContracts(result.Cases),
            Pagination = new PaginationResponse(request.Pagination as IPaginableRequest ?? paginable, result.Pagination.RecordsTotalSize)
        };
    }

    static List<int>? getStatesFilter(int? filterId)
        => filterId switch
        {
            1 => [1, 2, 3, 4, 5, 8, 9],
            2 => [1, 2, 8],
            3 => [3],
            4 => [4],
            5 => [5],
            _ => throw new NotImplementedException($"Filter {filterId} is not implemented")
        };

    static readonly List<Paginable.MapperField> sortingMapper =
    [
        new ("stateUpdated", "StateUpdatedOn"),
        new ("customerName", "Name")
    ];
}
