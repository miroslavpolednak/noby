using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementsListHandler
    : IRequestHandler<Dto.GetSalesArrangementListMediatrRequest, GetSalesArrangementListResponse>
{
    public async Task<GetSalesArrangementListResponse> Handle(Dto.GetSalesArrangementListMediatrRequest request, CancellationToken cancellation)
    {
        // vsechny SA states
        var availableStates = await _codebookService.SalesArrangementStates(cancellation);

        // pokud je pozadavek na konkretni stavy
        if (!request.Request.States.All(t => availableStates.Any(x => x.Id == t)))
            throw new CisNotFoundException(18006, $"SalesArrangementState does not exist.");

        // kontrola existence noveho stavu
        var requiredStates = getRequiredStates(request.Request.States, availableStates);

        var listData = await _repository.GetSalesArrangements(request.Request.CaseId, requiredStates, cancellation);

        GetSalesArrangementListResponse model = new();
        model.SalesArrangements.AddRange(listData);
        return model;
    }

    private static IEnumerable<int> getRequiredStates(IEnumerable<int> states, List<SalesArrangementStateItem> availableStates)
    {
        if (states.Any()) 
            return states;
        else if (_defaultStatesToQuery is null)
            _defaultStatesToQuery = availableStates.Where(t => t.Id != (int)CIS.Foms.Enums.SalesArrangementStates.Cancelled).Select(t => t.Id).ToArray();
        return _defaultStatesToQuery;
    }

    private readonly ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementsListHandler> _logger;

    private static int[]? _defaultStatesToQuery;

    public GetSalesArrangementsListHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementsListHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
