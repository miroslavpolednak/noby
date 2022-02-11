namespace FOMS.Api.SharedHandlers;

internal sealed class SharedCreateSalesArrangementHandler
    : IRequestHandler<Requests.SharedCreateSalesArrangementRequest, int>
{
    public async Task<int> Handle(Requests.SharedCreateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        _logger.SharedCreateSalesArrangementStarted(request);

        try
        {
            int salesArrangementId = ServiceCallResult.Resolve<int>(await _salesArrangementService.CreateSalesArrangement(request.CaseId, request.SalesArrangementTypeId, request.OfferId, cancellationToken));
            _logger.EntityCreated("SalesArrangement", salesArrangementId);
            return salesArrangementId;
        }
        catch (CisArgumentException ex)
        {
            // rethrow to be catched by validation middleware
            throw new CisValidationException(ex);
        }
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ILogger<SharedCreateSalesArrangementHandler> _logger;

    public SharedCreateSalesArrangementHandler(
        ILogger<SharedCreateSalesArrangementHandler> logger,
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _logger = logger;
        _salesArrangementService = salesArrangementService;
    }
}
