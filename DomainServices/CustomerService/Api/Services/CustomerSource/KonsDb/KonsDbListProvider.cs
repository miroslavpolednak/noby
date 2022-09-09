using System.Data;
using CIS.Core.Data;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

[ScopedService, SelfService]
public class KonsDbListProvider
{
    private readonly IConnectionProvider _connectionProvider;

    public KonsDbListProvider(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<IEnumerable<CustomerListItem>> GetList(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        var partners = await GetPartnerList(partnerIds, cancellationToken);

        return partners.Select(p => new CustomerListItem
        {
            Identity = new Identity(p.PartnerId, IdentitySchemes.Mp),
            NaturalPerson = p.ToNaturalPersonBasicInfo(),
            IdentificationDocument = p.ToIdentificationDocument()
        });
    }

    private Task<IEnumerable<Partner>> GetPartnerList(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetList, new { partnerIds }, cancellationToken: cancellationToken);

        return _connectionProvider.ExecuteDapperQuery(ListQuery, cancellationToken);

        Task<IEnumerable<Partner>> ListQuery(IDbConnection conn) => conn.QueryAsync<Partner>(command);
    }
}