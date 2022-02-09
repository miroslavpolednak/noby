using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class GetSalesArrangementsByCaseIdHandler
    : IRequestHandler<Dto.GetSalesArrangementsByCaseIdMediatrRequest, GetSalesArrangementsByCaseIdResponse>
{
    public async Task<GetSalesArrangementsByCaseIdResponse> Handle(Dto.GetSalesArrangementsByCaseIdMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementsByCaseIdHandler), request.Request.CaseId);

        // vsechny SA states
        var availableStates = await _codebookService.SalesArrangementStates();

        // pokud je pozadavek na konkretni stavy
        if (request.Request.States.Any(t => !availableStates.Any(x => x.Id == t)))
            throw new CisNotFoundException(16006, $"SalesArrangementState does not exist.");

        // kontrola existence noveho stavu
        int[] requiredStates = (request.Request.States.Any() ? request.Request.States.Select(t => t) : availableStates.Where(t => t.Id != _canceledSaState).Select(t => t.Id)).ToArray();

        var listData = await _repository.GetSalesArrangements(request.Request.CaseId, requiredStates, cancellation);

        GetSalesArrangementsByCaseIdResponse model = new();
        model.SalesArrangements.AddRange(listData);
        return model;
    }

    //TODO jak poznam ID stavu ZRUSENO?
    const int _canceledSaState = 3;

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<GetSalesArrangementsByCaseIdHandler> _logger;

    public GetSalesArrangementsByCaseIdHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<GetSalesArrangementsByCaseIdHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
