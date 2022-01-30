namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseStateHandler
    : IRequestHandler<Dto.UpdateCaseStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.UpdateCaseStateStart(request.CaseId, request.State);

        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.CaseId, cancellation);

        // overit ze case state existuje
        if (!(await _codebookService.CaseStates()).Any(t => t.Id == request.State))
            throw new CisNotFoundException(13011, nameof(request.State), request.State);

        // update v DB
        await _repository.UpdateCaseState(request.CaseId, request.State, cancellation);

        _logger.RequestHandlerFinished(nameof(UpdateCaseStateHandler));

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseStateHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}

