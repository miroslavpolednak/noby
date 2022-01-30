namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class ValidateSalesArrangementHandler
    : IRequestHandler<Dto.ValidateSalesArrangementMediatrRequest, Contracts.ValidateSalesArrangementResponse>
{
    public async Task<Contracts.ValidateSalesArrangementResponse> Handle(Dto.ValidateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        return null;
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<ValidateSalesArrangementHandler> _logger;
    private readonly Eas.IEasClient _easClient;

    public ValidateSalesArrangementHandler(
        Eas.IEasClient easClient,
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<ValidateSalesArrangementHandler> logger)
    {
        _easClient = easClient;
        _repository = repository;
        _logger = logger;
    }
}
