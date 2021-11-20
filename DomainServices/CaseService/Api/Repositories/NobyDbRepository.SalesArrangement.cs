using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;
using Dapper.Contrib.Extensions;

namespace DomainServices.CaseService.Api.Repositories;

internal partial class NobyDbRepository
{
    public async Task<int> CreateSalesArrangement(Dto.SalesArrangement.CreateSalesArrangementModel model)
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
