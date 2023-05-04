using CIS.Core.Security;
using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using _CS = DomainServices.CaseService.Contracts;

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

        // zavolat BE sluzbu
        var result = await _caseService.SearchCases(paginable, _userAccessor.User!.Id, getStatesFilter(request.FilterId), request.Term, cancellationToken);
        
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
            1 => new List<int>() { 1, 2 },
            2 => new List<int>() { 3 },
            3 => new List<int>() { 4 },
            4 => new List<int>() { 5 },
            _ => null
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
