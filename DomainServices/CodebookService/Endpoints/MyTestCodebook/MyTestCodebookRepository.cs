using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;

namespace DomainServices.CodebookService.Endpoints.MyTestCodebook;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService] //! nutne atributy pro registraci do DI
public class MyTestCodebookRepository 
    : DapperBaseRepository<MyTestCodebookRepository> // base Dapper repo - obsahuje helper metody jako WithConnection
{
    public MyTestCodebookRepository(ILogger<MyTestCodebookRepository> logger, IConnectionProvider<MyTestCodebookRepository> factory)
        : base(logger, factory)
    {
    }

    /// <summary>
    /// Hardcoded mock
    /// </summary>
    public List<Contracts.GenericCodebookItem> GetList()
        => new()
            {
                new() { Id = 1, Name = "Item 1" },
                new() { Id = 2, Name = "Item 2" }
            };

    /// <summary>
    /// Ukazka dotazu do databaze pomoci Dapper
    /// </summary>
    public async Task<List<Contracts.GenericCodebookItem>> GetListFromDatabase()
        => await WithConnection(async c => (await c.QueryAsync<Contracts.GenericCodebookItem>("SELECT Id, ServiceName 'Name' FROM ServiceDiscovery")).AsList());
}
