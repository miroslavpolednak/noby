using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.IncomeAbroadTypes;

namespace DomainServices.CodebookService.Endpoints.IncomeAbroadTypes;

public class IncomeAbroadTypesHandler
    : IRequestHandler<IncomeAbroadTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(IncomeAbroadTypesRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!      
        return new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 1, Name = "expat" },
            new GenericCodebookItem() { Id = 2, Name = "pendler" },
            new GenericCodebookItem() { Id = 3, Name = "cizinec s příjmem v zahraničí" },
            new GenericCodebookItem() { Id = 4, Name = "příjem ze zahr. S výkonem zam. V ČR" },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IncomeAbroadTypesHandler> _logger;

    public IncomeAbroadTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<IncomeAbroadTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
