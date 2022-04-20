using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.PropertySettlement;

namespace DomainServices.CodebookService.Endpoints.PropertySettlement;

public class PropertySettlementHandler
    : IRequestHandler<PropertySettlementRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(PropertySettlementRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!     
        return new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 1, Name = "Unknown" },
            new GenericCodebookItem() { Id = 2, Name = "Společné jmění manželů" },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<PropertySettlementHandler> _logger;

    public PropertySettlementHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<PropertySettlementHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
