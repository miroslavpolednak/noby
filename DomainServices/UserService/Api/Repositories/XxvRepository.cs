using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;

namespace DomainServices.UserService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class XxvRepository
    : DapperBaseRepository<XxvRepository>
{
    public XxvRepository(ILogger<XxvRepository> logger, IConnectionProvider factory)
        : base(logger, factory)
    { }

    public async Task<Dto.V33PmpUser?> GetUser(string login)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.V33PmpUser>("SELECT v33id, v33jmeno, v33prijmeni, v33cpm, v33icp FROM dbo.v33pmp_user WHERE v33cpm=@login", new { login = login }));
    }

    public async Task<Dto.V33PmpUser?> GetUser(int userId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.V33PmpUser>("SELECT v33id, v33jmeno, v33prijmeni, v33cpm, v33icp FROM dbo.v33pmp_user WHERE v33id=@id", new { id = userId }));
    }
}
