namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class CreateSalesArrangementParametersFactory
{
    private readonly ILogger<CreateSalesArrangementParametersFactory> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateSalesArrangementParametersFactory(IHttpContextAccessor httpContextAccessor, ILogger<CreateSalesArrangementParametersFactory> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public ICreateSalesArrangementParametersValidator CreateBuilder(long caseId, int salesArrangementTypeId)
    {
        var request = new DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest
        {
            CaseId = caseId,
            SalesArrangementTypeId = salesArrangementTypeId
        };

        return salesArrangementTypeId switch
        {
            //>= 1 and <= 5 => new MortgageBuilder(salesArrangementId, _logger),
            6 => new DrawingValidator(_logger, request, _httpContextAccessor),
            _ => throw new NotImplementedException($"Create Builder not implemented for SalesArrangementTypeId={salesArrangementTypeId}")
        };
    }
}