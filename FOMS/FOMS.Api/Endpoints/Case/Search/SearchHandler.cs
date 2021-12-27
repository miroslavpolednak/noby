﻿using CIS.Core.Results;

namespace FOMS.Api.Endpoints.Case.Handlers;

internal class SearchHandler
    : IRequestHandler<Dto.SearchRequest, Dto.SearchResponse>
{
    public async Task<Dto.SearchResponse> Handle(Dto.SearchRequest request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Search for user {userId}", _userAccessor.User.Id);

        // upravit razeni - pole na FE nejsou shodna s field v DB
        request.Pagination?.ChangeSortingFields(new List<(string Original, string ChangeTo)>
        {
            new ("CustomerName", "Name")
        });

        var result = resolveResult(await _caseService.SearchCases(request.Pagination ?? CIS.Core.Types.PaginableRequest.Default, _userAccessor.User.Id, request.State, request.Term, cancellationToken));

        _logger.LogDebug("Found {records} records", result.Pagination.RecordsTotalSize);

        // transform
        return new Dto.SearchResponse
        {
            Rows = await _converter.FromContracts(result.CaseInstances),
            Pagination = result.Pagination
        };
    }

    private DomainServices.CaseService.Contracts.SearchCasesResponse resolveResult(IServiceCallResult result) =>
       result switch
       {
           SuccessfulServiceCallResult<DomainServices.CaseService.Contracts.SearchCasesResponse> r => r.Model,
           _ => throw new NotImplementedException()
       };

    private readonly ILogger<SearchHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly CaseModelConverter _converter;
    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;

    public SearchHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<SearchHandler> logger,
        CaseModelConverter converter,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService)
    {
        _converter = converter;
        _userAccessor = userAccessor;
        _logger = logger;
        _caseService = caseService;
    }
}
