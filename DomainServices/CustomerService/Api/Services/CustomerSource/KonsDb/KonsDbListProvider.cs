using System.Data;
using CIS.Core.Data;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using CIS.Infrastructure.gRPC.CisTypes;
using Dapper;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;
using DomainServices.CustomerService.Contracts;

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
            NaturalPerson = CreateNaturalPerson(p),
            IdentificationDocument = CreateIdentificationDocument(p)
        });
    }

    private Task<IEnumerable<Partner>> GetPartnerList(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetList, new { partnerIds }, cancellationToken: cancellationToken);

        return _connectionProvider.ExecuteDapperQuery(ListQuery, cancellationToken);

        Task<IEnumerable<Partner>> ListQuery(IDbConnection conn) => conn.QueryAsync<Partner>(command);
    }

    private static NaturalPersonBasicInfo CreateNaturalPerson(Partner partner)
    {
        return new NaturalPersonBasicInfo
        {
            FirstName = partner.FirstName ?? string.Empty,
            LastName = partner.LastName ?? string.Empty,
            GenderId = partner.GenderId,
            BirthNumber = partner.BirthNumber ?? string.Empty,
            DateOfBirth = partner.BirthDate
        };
    }

    private static IdentificationDocument? CreateIdentificationDocument(Partner partner)
    {
        return new IdentificationDocument
        {
            Number = partner.IdentificationDocumentNumber ?? string.Empty,
            IdentificationDocumentTypeId = partner.IdentificationDocumentTypeId,
            IssuedOn = partner.IdentificationDocumentIssuedOn,
            IssuedBy = partner.IdentificationDocumentIssuedBy ?? string.Empty,
            IssuingCountryId = partner.IdentificationDocumentIssuingCountryId,
            ValidTo = partner.IdentificationDocumentValidTo
        };
    }
}