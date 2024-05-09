using CIS.Core.Data;
using CIS.Infrastructure.Data;
using DomainServices.ProductService.Api.Database.Models;
using SharedTypes.GrpcTypes;

namespace DomainServices.ProductService.Api.Database;

[ScopedService, SelfService]
internal sealed class LoanRepository
{
    private readonly IConnectionProvider _connectionProvider;
    
    public LoanRepository(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
    
    public Task<List<SearchProductsResponse.Types.SearchProductsItem>> SearchProducts(Identity? identity, CancellationToken cancellationToken)
    {
        string query = """
        select
            u.Id as CaseId,
            vu.VztahId as ContractRelationshipTypeId
        from [dbo].[VztahUver] vu
        inner join [dbo].[Partner] p on p.Id = vu.PartnerId
        inner join [dbo].[Uver] u on u.Id = vu.UverId
        where  vu.Neaktivni = 0 
               and u.Neaktivni = 0 
               and ((@schema=2 and p.KBId=@id) or (@schema=1 and p.Id=@id))

        union all

        select
            s.Id as CaseId,
            vs.VztahId as ContractRelationshipTypeId
        from [dbo].[VztahSporeni] vs
        inner join [dbo].[Partner] p on p.Id = vs.PartnerId
        inner join [dbo].[Sporeni] s on s.Id = vs.SporeniId 
        where  vs.Neaktivni = 0 and ((@schema=2 and p.KBId=@id) or (@schema=1 and p.Id=@id))
        """;

        if (identity is not null)
        {
            return _connectionProvider.ExecuteDapperRawSqlToListAsync<SearchProductsResponse.Types.SearchProductsItem>(query, new { schema = (int)identity.IdentityScheme, id = identity.IdentityId }, cancellationToken);
        }

        throw new NotImplementedException();
    }

    public Task<List<CovenantPhase>> GetCovenantPhases(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as CaseId,   
                   [Nazev] as Name,
                   [Poradi] as [Order],
                   [PoradiPismeno] as OrderLetter
            FROM [dbo].[TerminovnikFaze]
            WHERE [UverId] = @caseId
            """;

	    return _connectionProvider.ExecuteDapperRawSqlToListAsync<CovenantPhase>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<List<Obligation>> GetObligations(long caseId, CancellationToken cancellationToken)
    {
        const string Query =
            """
            SELECT [UverId] as ProductId, 
                   [UcelUveruInt] as LoanPurposeId,
                   [TypZavazku] as ObligationTypeId,
                   [Castka] as Amount,
                   [Veritel] as CreditorName,
                   [PredcisliUctu] as AccountNumberPrefix,
                   [CisloUctu] as AccountNumber,
                   [KodBanky] as BankCode,
                   [VariabilniSymbol] as VariableSymbol
            FROM [dbo].[Zavazky]
            WHERE [UverId] = @caseId AND [TypZavazku] <> 0 AND [Castka] > 0 AND [Veritel] IS NOT NULL
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<Obligation>(Query, param: new { caseId }, cancellationToken);
    }

    public Task<bool> RelationshipExists(long caseId, long partnerId, CancellationToken cancellationToken)
    { 
	    const string Query = "SELECT COUNT(1) from [dbo].[VztahUver] where [UverId] = @caseId and [PartnerId] = @partnerId and [Neaktivni] = 0";

        return _connectionProvider.ExecuteDapperScalarAsync<bool>(Query, param: new { caseId, partnerId }, cancellationToken);
    }
}