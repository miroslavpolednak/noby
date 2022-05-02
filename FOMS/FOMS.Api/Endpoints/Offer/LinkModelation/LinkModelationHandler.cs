using DomainServices.SalesArrangementService.Abstraction;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.LinkModelation;

internal class LinkModelationHandler
    : AsyncRequestHandler<LinkModelationRequest>
{
    protected override async Task Handle(LinkModelationRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkModelationHandler), request.SalesArrangementId);

        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken));

        // nalinkovat novou simulaci
        await _salesArrangementService.LinkModelationToSalesArrangement(request.SalesArrangementId, request.OfferId, cancellationToken);

        // smazat objekty uveru
        if (saInstance.Mortgage?.LoanRealEstates is not null && saInstance.Mortgage.LoanRealEstates.Any())
        {
            saInstance.Mortgage.LoanRealEstates.Clear();
            await _salesArrangementService.UpdateSalesArrangementParameters(new _SA.UpdateSalesArrangementParametersRequest
            {
                SalesArrangementId = request.SalesArrangementId,
                Mortgage = saInstance.Mortgage
            }, cancellationToken);
        }
    }

    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<LinkModelationHandler> _logger;

    public LinkModelationHandler(
        ISalesArrangementServiceAbstraction salesArrangementService,
        ILogger<LinkModelationHandler> logger)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
