using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;
using System.Linq;

namespace DomainServices.ProductService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal sealed class KonsDbRepository
    : DapperBaseRepository<NobyDbRepository>
{
    private const string _basicDetailSelect = "CisloSmlouvy, CilovaCastka, ZadaStatniPremii, StavUctuCelkem, UrokovaSazbaSporeni, DatumUzavreniSmlouvy";

    public KonsDbRepository(ILogger<NobyDbRepository> logger, IConnectionProvider<KonsDbRepository> factory)
        : base(logger, factory) { }

    public async Task<Dto.HousingSavingsBasicDetailModel> GetSavingsBasicDetail(long productInstanceId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.HousingSavingsBasicDetailModel>("SELECT " + _basicDetailSelect + " FROM dbo.Sporeni WHERE Id=@id", new { id = productInstanceId }));
    }

    public async Task<Dto.HousingSavingsFullDetailModel> GetSavingsFullDetail(long productInstanceId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.HousingSavingsFullDetailModel>("SELECT " + _basicDetailSelect + ", MesicniSplatka FROM dbo.Sporeni WHERE Id=@id", new { id = productInstanceId }));
    }

    public async Task<Dto.ProductInstanceListModel?> GetSavingsListItem(long caseId)
    {
        return await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Dto.ProductInstanceListModel>("SELECT TOP 1 ProductInstanceId, ProductInstanceTypeId 'ProductInstanceTypeId', State FROM dbo.Sporeni WHERE Id=@id", new { id = caseId }));
    }

    //TODO jak najit uvery k SS podle CaseId?
    public async Task<List<Dto.ProductInstanceListModel>> GetSavingsLoanListItems(long caseId)
    {
        return await WithConnection(async c => (await c.QueryAsync<Dto.ProductInstanceListModel>("SELECT ProductInstanceId, ProductInstanceTypeId 'ProductInstanceTypeId', State FROM dbo.Sporeni WHERE SporeniId=@id", new { id = caseId })).ToList());
    }
}
