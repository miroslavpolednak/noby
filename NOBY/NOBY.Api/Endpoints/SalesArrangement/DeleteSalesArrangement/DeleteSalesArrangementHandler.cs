namespace NOBY.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : AsyncRequestHandler<DeleteSalesArrangementRequest>
{
    protected override async Task Handle(DeleteSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        await _salesArrangementService.DeleteSalesArrangement(request.SalesArrangementId, cancellationToken);
    }

    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients _salesArrangementService;

    public DeleteSalesArrangementHandler(DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClients salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
