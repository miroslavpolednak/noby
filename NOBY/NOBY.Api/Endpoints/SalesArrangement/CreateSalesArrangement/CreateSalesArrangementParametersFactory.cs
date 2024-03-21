using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement;

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
        var aggregate = new BuilderValidatorAggregate(new()
        {
            CaseId = caseId,
            SalesArrangementTypeId = salesArrangementTypeId
        },
        _httpContextAccessor);

        return (SalesArrangementTypes)salesArrangementTypeId switch
        {
            //>= 1 and <= 5 => new MortgageBuilder(salesArrangementId, _logger),
            SalesArrangementTypes.Drawing => new DrawingValidator(aggregate),
            SalesArrangementTypes.GeneralChange => new GeneralChangeValidator(aggregate),
            SalesArrangementTypes.HUBN => new HUBNValidator(aggregate),
            SalesArrangementTypes.CustomerChange => new CustomerChangeValidator(aggregate),
            SalesArrangementTypes.CustomerChange3602A => new CustomerChange3602AValidator(aggregate),
            SalesArrangementTypes.CustomerChange3602B => new CustomerChange3602BValidator(aggregate),
            SalesArrangementTypes.CustomerChange3602C => new CustomerChange3602CValidator(aggregate),
            SalesArrangementTypes.MortgageRetention => new RetentionValidator(aggregate),
            _ => throw new NotImplementedException($"Create Builder not implemented for SalesArrangementTypeId={salesArrangementTypeId}")
        };
    }
}