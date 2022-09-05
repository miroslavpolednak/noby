using CIS.Core.Data;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;
using DomainServices.CustomerService.Contracts;
using System.Data;
using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

[ScopedService, SelfService]
public class KonsDbSearchProvider
{
    private readonly IConnectionProvider _connectionProvider;

    public KonsDbSearchProvider(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<IEnumerable<SearchCustomersItem>> Search(SearchCustomersRequest searchRequest, CancellationToken cancellationToken)
    {
        var contacts = await SearchByContacts(searchRequest.Email, searchRequest.PhoneNumber, cancellationToken);

        var request = new SearchRequest
        {
            PartnerIds = PartnerIdAsEnumerable(searchRequest.Identity).Concat(contacts).ToList(),
            FirstName = GetValueOrNull(searchRequest.NaturalPerson?.FirstName),
            LastName = GetValueOrNull(searchRequest.NaturalPerson?.LastName),
            BirthNumber = GetValueOrNull(searchRequest.NaturalPerson?.BirthNumber),
            DateOfBirth = searchRequest.NaturalPerson?.DateOfBirth,
            DocumentNumber = GetValueOrNull(searchRequest.IdentificationDocument?.Number),
            DocumentTypeId = searchRequest.IdentificationDocument?.IdentificationDocumentTypeId,
            DocumentIssuingCountryId = searchRequest.IdentificationDocument?.IssuingCountryId
        };

        var customers = await SearchCustomers(request, cancellationToken);

        return customers.Select(c => new SearchCustomersItem
        {
            NaturalPerson = CreateNaturalPerson(c),
            Street = c.Street ?? string.Empty,
            City = c.City ?? string.Empty,
            Postcode = c.PostCode ?? string.Empty
        });

        static IEnumerable<long> PartnerIdAsEnumerable(Identity? identity)
        {
            if (identity is null)
                yield break;

            yield return identity.IdentityId;
        }

        static string? GetValueOrNull(string? str) => string.IsNullOrWhiteSpace(str) ? null : str;
    }

    private Task<IEnumerable<Partner>> SearchCustomers(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        if (searchRequest.IsEmptyRequest)
            return Task.FromResult(Enumerable.Empty<Partner>());

        var command = new CommandDefinition(Sql.SqlScripts.SearchCustomers, parameters: searchRequest, cancellationToken: cancellationToken);

        return _connectionProvider.ExecuteDapperQuery(SearchQuery, cancellationToken);

        Task<IEnumerable<Partner>> SearchQuery(IDbConnection conn) => conn.QueryAsync<Partner>(command);
    }

    private Task<IEnumerable<long>> SearchByContacts(string? email, string? phoneNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
            return Task.FromResult(Enumerable.Empty<long>());

        var command = new CommandDefinition(Sql.SqlScripts.SearchCustomersContacts, parameters: new { email, phoneNumber }, cancellationToken: cancellationToken);

       return _connectionProvider.ExecuteDapperQuery(SearchContacts, cancellationToken);

        Task<IEnumerable<long>> SearchContacts(IDbConnection conn) => conn.QueryAsync<long>(command);
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
}