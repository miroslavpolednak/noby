namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementDataHandler
    : IRequestHandler<Dto.UpdateSalesArrangementDataMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementDataMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementDataHandler), request.Request.SalesArrangementId);

        await _repository.UpdateSalesArrangementData(request.Request.SalesArrangementId, request.Request.Data);

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
