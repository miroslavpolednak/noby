using Dapper;
using CIS.Infrastructure.Data;
using CIS.Core.Data;
using Dapper.Contrib.Extensions;

namespace DomainServices.CaseService.Api.Repositories;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal partial class NobyDbRepository
    : DapperBaseRepository<NobyDbRepository>
{
    public NobyDbRepository(ILogger<NobyDbRepository> logger, IConnectionProvider<NobyDbRepository> factory)
        : base(logger, factory) { }

    public async Task<bool> IsExistingCase(long caseId)
        => await WithConnection(async c => await c.QueryFirstOrDefaultAsync("SELECT TOP 1 CaseId FROM CaseInstance WHERE CaseId=@id", new { id = caseId }) != null);

    public async Task CreateCase(Dto.CaseService.CreateCaseModel model)
        => await WithConnection(async c => await c.InsertAsync(model));

    public async Task<Contracts.GetCaseDetailResponse> GetCaseDetail(long caseId)
        => await WithConnection(async c => await c.QueryFirstOrDefaultAsync<Contracts.GetCaseDetailResponse>("SELECT * FROM dbo.CaseInstance WHERE CaseId=@id", new { id = caseId }));

    public async Task<Contracts.GetCaseListResponse> GetCaseList(int partyId, int? state, CIS.Infrastructure.gRPC.CisTypes.PaginationRequest pagination)
    {
        string where = " WHERE A.PartyId=@id" + (state.HasValue ? " AND A.State=@state" : "");

        string sqlCount = "SELECT COUNT(*) FROM dbo.CaseInstance A" + where;
        string sqlMain = ";SELECT * FROM dbo.CaseInstance A" + where + $" ORDER BY A.InsertTime OFFSET {(pagination.PageSize * (pagination.RecordOffset - 1))} ROWS FETCH NEXT {pagination.PageSize} ROWS ONLY;";

        Contracts.GetCaseListResponse result = new();

        return await WithConnection(async c =>
        {
            using (var multi = await c.QueryMultipleAsync(sqlCount + sqlMain, new { id = partyId, state = state }))
            {
                result.Pagination = pagination.CreateResponse(multi.ReadFirst<int>());
                result.CaseInstances.AddRange(multi.Read<Contracts.CaseListModel>());
            }

            return result;
        });
    }

    public async Task LinkOwnerToCase(long caseId, int partyId)
        => await WithConnection(async c => await c.ExecuteAsync("UPDATE dbo.CaseInstance SET PartyId=@partyId WHERE CaseId=@id", new { partyId = partyId, id = caseId }));

    public async Task UpdateCaseData(long caseId, string contractNumber)
        => await WithConnection(async c => await c.ExecuteAsync("UPDATE dbo.CaseInstance SET ContractNumber=@contractNo WHERE CaseId=@id", new { contractNo = contractNumber, id = caseId }));

    public async Task UpdateCaseState(long caseId, int state)
        => await WithConnection(async c => await c.ExecuteAsync("UPDATE dbo.CaseInstance SET State=@state WHERE CaseId=@id", new { state = state, id = caseId }));

    public async Task UpdateCaseCustomer(long caseId, int identityId, string firstName, string name, DateOnly? birthDate)
        => await WithConnection(async c => await c.ExecuteAsync(
            "UPDATE dbo.CaseInstance SET DateOfBirthNaturalPerson=@birthDate, Name=@name, FirstNameNaturalPerson=@firstName, CustomerIdentityId=@identityId WHERE CaseId=@caseId", 
            new { identityId, caseId, firstName, name, birthDate }));
}
