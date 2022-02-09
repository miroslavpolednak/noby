namespace DomainServices.CaseService.Api.Handlers;

internal class UpdateCaseCustomerHandler
    : IRequestHandler<Dto.UpdateCaseCustomerMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateCaseCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateCaseCustomerHandler), request.Request.CaseId);

        // zjistit zda existuje case
        await _repository.EnsureExistingCase(request.Request.CaseId, cancellation);
        //TODO zkontrolovat existenci klienta?

        // ulozit do DB
        await _repository.UpdateCaseCustomer(request.Request.CaseId, request.Request.Customer, cancellation);

        _logger.RequestHandlerFinished(nameof(UpdateCaseCustomerHandler));

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.CaseServiceRepository _repository;
    private readonly ILogger<UpdateCaseCustomerHandler> _logger;

    public UpdateCaseCustomerHandler(
        Repositories.CaseServiceRepository repository,
        ILogger<UpdateCaseCustomerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
