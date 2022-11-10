using CIS.Core.Security;
using CIS.Core.Types;
using CIS.Infrastructure.WebApi.Types;
using _CS = DomainServices.CaseService.Contracts;

namespace NOBY.Api.Endpoints.Cases.Search;

internal class SearchHandler
    : IRequestHandler<SearchRequest, SearchResponse>
{
    public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        _logger.SearchPaginableSettings(paginable);
        
        // zavolat BE sluzbu
        var result = ServiceCallResult.ResolveAndThrowIfError<_CS.SearchCasesResponse>(await _caseService.SearchCases(paginable, _userAccessor.User!.Id, getStatesFilter(request.FilterId), request.Term, cancellationToken));
        _logger.FoundItems(result.Pagination.RecordsTotalSize, nameof(_CS.Case));

        // transform
        return new SearchResponse
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

    private readonly ILogger<SearchHandler> _logger;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly CasesModelConverter _converter;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;

    public SearchHandler(
        ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        CasesModelConverter converter,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService)
    {
        _converter = converter;
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
