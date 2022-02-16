using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;

namespace DomainServices.ProductService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class NobyDbRepository
    : DapperBaseRepository<NobyDbRepository>
{
    public NobyDbRepository(ILogger<NobyDbRepository> logger, IConnectionProvider<NobyDbRepository> factory)
        : base(logger, factory) { }

    /// <summary>
    /// Vraci true, pokud CaseId existuje v databazy NOBY
    /// </summary>
    public async Task<bool> IsExistingCase(long caseId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync("SELECT TOP 1 CaseId FROM CaseInstance WHERE CaseId=@id", new { id = caseId }) != null);
    }

    public async Task<Dto.CaseModel> GetCase(long caseId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.CaseModel>("SELECT TOP 1 * FROM CaseInstance WHERE CaseId=@id", new { id = caseId }));
    }
}
