namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementDataHandler
    : IRequestHandler<Dto.UpdateSalesArrangementMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementDataHandler), request.Request.SalesArrangementId);

        await _repository.UpdateSalesArrangement(request.Request.SalesArrangementId, request.Request.ContractNumber, cancellation);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementDataHandler> _logger;

    public UpdateSalesArrangementDataHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementDataHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
