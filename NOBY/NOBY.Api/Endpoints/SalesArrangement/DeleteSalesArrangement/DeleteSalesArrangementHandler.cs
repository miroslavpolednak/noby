namespace NOBY.Api.Endpoints.SalesArrangement.DeleteSalesArrangement;

internal sealed class DeleteSalesArrangementHandler
    : AsyncRequestHandler<DeleteSalesArrangementRequest>
{
    protected override async Task Handle(DeleteSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        await _salesArrangementService.DeleteSalesArrangement(request.SalesArrangementId, cancellationToken);
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public DeleteSalesArrangementHandler(DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
