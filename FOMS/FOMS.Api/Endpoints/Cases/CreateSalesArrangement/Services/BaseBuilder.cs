namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal abstract class BaseBuilder
{
    protected readonly DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest _request;
    protected readonly ILogger<CreateSalesArrangementParametersFactory> _logger;

    public BaseBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request)
    {
        _logger = logger;
        _request = request;
    }
}
