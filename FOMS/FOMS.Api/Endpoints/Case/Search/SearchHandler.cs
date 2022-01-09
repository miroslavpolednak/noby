using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, Dto.SearchResponse>
{
    public async Task<Dto.SearchResponse> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Search for user {userId}", _userAccessor.User.Id);

        // strankovani a razeni
        var pagination = request.Pagination?.WithSortFields(_sortingFieldsMapper) ?? CIS.Core.Types.PaginableRequest.Create("CreatedTime", true);
        //TODO tady je otazka co je vlastne filtrovani podle stavu???
        request.State = 1;

        _logger.LogDebug("Pagination {RecordOffset}/{PageSize} - {field}/{descending}", request.Pagination?.RecordOffset, request.Pagination?.PageSize, request.Pagination?.Sort?.First().Field, request.Pagination?.Sort?.First().Descending);

        var result = resolveResult(await _caseService.SearchCases(pagination, _userAccessor.User.Id, request.State, request.Term, cancellationToken));

        _logger.LogDebug("Found {records} records", result.Pagination.RecordsTotalSize);

        //TODO pouze docasne kvuli dashboardu
        var rows = await _converter.FromContracts(result.CaseInstances);
        foreach (var row in rows)
        {
            var sa = ServiceCallResult.Resolve<DomainServices.SalesArrangementService.Contracts.GetSalesArrangementsByCaseIdResponse>(await _salesArrangementService.GetSalesArrangementsByCaseId(row.CaseId, new int[0] { }));
            row.SalesArrangementId = sa.SalesArrangements.FirstOrDefault()?.SalesArrangementId;
        }

        // transform
        return new Dto.SearchResponse
        {
            Rows = rows,
            Pagination = result.Pagination.WithSortFields(_sortingFieldsMapper)
        };
    }

    private DomainServices.CaseService.Contracts.SearchCasesResponse resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.CaseService.Contracts.SearchCasesResponse> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private static List<(string Original, string ChangeTo)> _sortingFieldsMapper = new()
    {
        new ("createdBy", "CreatedTime"),
        new ("customerName", "Name")
    };

    private readonly ILogger<SearchHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly CaseModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public SearchHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        CaseModelConverter converter,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _salesArrangementService = salesArrangementService;
        _converter = converter;
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
