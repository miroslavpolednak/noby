using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.EmploymentTypes;

namespace DomainServices.CodebookService.Endpoints.EmploymentTypes;

public class EmploymentTypesHandler
    : IRequestHandler<EmploymentTypesRequest, List<GenericCodebookItem>>
{
    public async Task<List<GenericCodebookItem>> Handle(EmploymentTypesRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!           
        return new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 1, Name = "pronájem existující" },
            new GenericCodebookItem() { Id = 2, Name = "pronájem budoucí" },
            new GenericCodebookItem() { Id = 3, Name = "prac.poměr - doba určitá" },
            new GenericCodebookItem() { Id = 4, Name = "prac.poměr - doba neurčitá" },
            new GenericCodebookItem() { Id = 5, Name = "prac.poměr - dpp" },
            new GenericCodebookItem() { Id = 6, Name = "prac.poměr - dpc" },
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<EmploymentTypesHandler> _logger;

    public EmploymentTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<EmploymentTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
