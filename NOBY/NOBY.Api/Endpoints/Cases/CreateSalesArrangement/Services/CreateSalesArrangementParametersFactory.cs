using CIS.Foms.Types.Enums;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
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

        return (SalesArrangementTypes)salesArrangementTypeId switch
        {
            //>= 1 and <= 5 => new MortgageBuilder(salesArrangementId, _logger),
            SalesArrangementTypes.Drawing => new DrawingValidator(_logger, request, _httpContextAccessor),
            SalesArrangementTypes.GeneralChange => new GeneralChangeValidator(_logger, request, _httpContextAccessor),
            SalesArrangementTypes.HUBN => new HUBNValidator(_logger, request, _httpContextAccessor),
            SalesArrangementTypes.CustomerChange => new CustomerChangeValidator(_logger, request, _httpContextAccessor),
            _ => throw new NotImplementedException($"Create Builder not implemented for SalesArrangementTypeId={salesArrangementTypeId}")
        };
    }
}