using CIS.Core.Security;
using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using SharedTypes.Enums;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class SearchCasesHandler
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
            1 => new List<int>() { 1, 2, 3, 4, 5, 8, 9 },
            2 => new List<int>() { 1, 2, 8 },
            3 => new List<int>() { 3 },
            4 => new List<int>() { 4 },
            5 => new List<int>() { 5 },
            _ => throw new NotImplementedException($"Filter {filterId} is not implemented")
        };

    static List<Paginable.MapperField> sortingMapper = new()
    {
        new ("stateUpdated", "StateUpdatedOn"),
        new ("customerName", "Name")
    };

    private readonly ICurrentUserAccessor _userAccessor;
    private readonly CasesModelConverter _converter;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public SearchCasesHandler(
        ICurrentUserAccessor userAccessor,
        CasesModelConverter converter,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _converter = converter;
        _userAccessor = userAccessor;
        _caseService = caseService;
    }
}
