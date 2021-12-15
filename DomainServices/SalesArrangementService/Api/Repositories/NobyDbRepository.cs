using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;
using Dapper.Contrib.Extensions;

namespace DomainServices.SalesArrangementService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class NobyDbRepository
    : DapperBaseRepository<NobyDbRepository>
{
    public NobyDbRepository(ILogger<NobyDbRepository> logger, IConnectionProvider<NobyDbRepository> factory)
        : base(logger, factory) { }

    public async Task<Dto.CaseModel> GetCaseDetail(long caseId)
        => await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.CaseModel>("SELECT * FROM dbo.CaseInstance WHERE CaseId=@id", new { id = caseId }));

    public async Task<int> CreateSalesArrangement(Dto.CreateSalesArrangementModel model)
        => await WithConnection(async c => await c.InsertAsync(model));

    public async Task<Contracts.SalesArrangementListModel> GetSalesArrangement(int salesArrangementId)
        => await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Contracts.SalesArrangementListModel>("SELECT SalesArrangementId, CaseId, OfferInstanceId, ProductInstanceId, SalesArrangementType, SalesArrangementStatus, InsertUserId, InsertTime FROM dbo.SalesArrangement WHERE SalesArrangementId=@id", new { id = salesArrangementId }));

    public async Task UpdateSalesArrangementState(int salesArrangementId, int state)
        => await WithConnection(async c => await c.ExecuteAsync("UDPATE dbo.SalesArrangement SET State=@state WHERE SalesArrangementId=@id", new { id = salesArrangementId, state }));

    public async Task<Contracts.GetSalesArrangementDetailResponse> GetSalesArrangementDetail(int salesArrangementId)
        => await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Contracts.GetSalesArrangementDetailResponse>("SELECT * FROM dbo.SalesArrangement WHERE SalesArrangementId=@id", new { id = salesArrangementId }));

    public async Task<List<Contracts.SalesArrangementListModel>> GetSalesArrangementsByCaseId(long caseId)
        => await WithConnection(async c => (await c.QueryAsync<Contracts.SalesArrangementListModel>("SELECT SalesArrangementId, CaseId, OfferInstanceId, ProductInstanceId, SalesArrangementType, SalesArrangementStatus, InsertUserId, InsertTime FROM dbo.SalesArrangement WHERE CaseId=@id", new { id = caseId })).ToList());
}
