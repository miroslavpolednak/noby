﻿using CIS.Core.Types;
using DomainServices.CaseService.Contracts;

namespace DomainServices.CaseService.Api.Handlers;

internal class SearchCasesHandler
    : IRequestHandler<Dto.SearchCasesMediatrRequest, SearchCasesResponse>
{
    /// <summary>
    /// Seznam Case s moznosti strankovani. Vetsinou pro daneho uzivatele.
    /// </summary>
    public async Task<SearchCasesResponse> Handle(Dto.SearchCasesMediatrRequest request, CancellationToken cancellation)
    {
        // vytvorit informaci o strankovani / razeni
        var paginable = Paginable
            .FromRequest(request.Request.Pagination)
            .EnsureAndTranslateSortFields(sortingMapper);

        _logger.LogDebug("Search cases {paginable}", paginable);

        // dotaz do DB
        var model = await _repository.GetCaseList(paginable, request.Request.CaseOwnerUserId, request.Request.State, request.Request.SearchTerm, cancellation);
        _logger.LogDebug("Found {RecordsTotalSize} cases", model.RecordsTotalSize);

        var result = new SearchCasesResponse()
        {
            Pagination = new CIS.Infrastructure.gRPC.CisTypes.PaginationResponse(request.Request.Pagination as IPaginableRequest ?? paginable, model.RecordsTotalSize)
        };
        result.CaseInstances.AddRange(model.CaseInstances);

        return result;
    }

    // povolena pole pro sortovani
    private static List<Paginable.MapperField> sortingMapper = new()
    { 
        new("StateUpdatedOn", "StateUpdateTime") 
    };

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;
    
    public SearchCasesHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
