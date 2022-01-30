namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseDataHandler
    : IRequestHandler<Dto.UpdateCaseDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCaseDataHandler), request.Request.CaseId);

        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.Request.CaseId, cancellation);

        // zkontrolovat ProdInstType
        if (!(await _codebookService.ProductInstanceTypes()).Any(t => t.Id == request.Request.Data.ProductInstanceTypeId))
            throw new CisNotFoundException(13014, nameof(request.Request.Data.ProductInstanceTypeId), request.Request.Data.ProductInstanceTypeId);

        // ulozit do DB
        await _repository.UpdateCaseData(request.Request.CaseId, request.Request.Data, cancellation);

        _logger.RequestHandlerFinished(nameof(UpdateCaseDataHandler));

        return new Google.Protobuf.WellKnownTypes.Empty();
    }
    
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<CreateCaseHandler> _logger;

    public UpdateCaseDataHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CaseServiceRepository repository,
        ILogger<CreateCaseHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _logger = logger;
    }
}
