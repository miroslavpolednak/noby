using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementsListHandler
    : IRequestHandler<Dto.GetSalesArrangementListMediatrRequest, GetSalesArrangementListResponse>
{
    public async Task<GetSalesArrangementListResponse> Handle(Dto.GetSalesArrangementListMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementsListHandler), request.Request.CaseId);

        // vsechny SA states
        var availableStates = await _codebookService.SalesArrangementStates();

        // pokud je pozadavek na konkretni stavy
        if (!request.Request.States.All(t => availableStates.Any(x => x.Id == t)))
            throw new CisNotFoundException(16006, $"SalesArrangementState does not exist.");

        // kontrola existence noveho stavu
        int[] requiredStates = request.Request.States.Any() ? request.Request.States.Select(t => t).ToArray() : new[] { (int)CIS.Core.Enums.SalesArrangementStates.Cancelled };

        var listData = await _repository.GetSalesArrangements(request.Request.CaseId, requiredStates, cancellation);

        GetSalesArrangementListResponse model = new();
        model.SalesArrangements.AddRange(listData);
        return model;
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementsListHandler> _logger;

    public GetSalesArrangementsListHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementsListHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
