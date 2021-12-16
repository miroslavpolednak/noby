namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class UpdateSalesArrangementStateHandler
    : IRequestHandler<Dto.UpdateSalesArrangementStateMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.UpdateSalesArrangementStateMediatrRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Update state of #{id} to {state}", request.SalesArrangementId, request.State);

        await _repository.GetSalesArrangement(request.SalesArrangementId);
        await _repository.UpdateSalesArrangementState(request.SalesArrangementId, request.State);

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    private readonly Repositories.SalesArrangementServiceRepository _repository;
    private readonly ILogger<UpdateSalesArrangementStateHandler> _logger;

    public UpdateSalesArrangementStateHandler(
        Repositories.SalesArrangementServiceRepository repository,
        ILogger<UpdateSalesArrangementStateHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
