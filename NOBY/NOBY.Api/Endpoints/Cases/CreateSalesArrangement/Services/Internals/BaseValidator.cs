namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

internal abstract class BaseValidator
{
    protected readonly DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest _request;
    protected readonly ILogger<CreateSalesArrangementParametersFactory> _logger;

    public BaseValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request)
    {
        _logger = logger;
        _request = request;
    }
}
