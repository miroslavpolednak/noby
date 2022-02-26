namespace FOMS.Api.Endpoints.SalesArrangement.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<Dto.CustomerListItem>>
{
    public async Task<List<Dto.CustomerListItem>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomersHandler), request.SalesArrangementId);

        _salesArrangement
    }

    private readonly ILogger<GetCustomersHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangement;

    public GetCustomersHandler(
        ILogger<GetCustomersHandler> logger, 
        DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangement)
    {
        _salesArrangement = salesArrangement;
        _logger = logger;
    }
}