using System.Data;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Api.Services.KonsDb.Dto;

namespace DomainServices.CustomerService.Api.Services.KonsDb;

[ScopedService, SelfService]
public class KonsDbDetailProvider
{
    private readonly IConnectionProvider _connectionProvider;
    private readonly ICodebookServiceClients _codebook;

    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;

    public KonsDbDetailProvider(IConnectionProvider connectionProvider, ICodebookServiceClients codebook)
    {
        _connectionProvider = connectionProvider;
        _codebook = codebook;
    }

    public async Task<CustomerDetailResponse> GetDetail(long partnerId, CancellationToken cancellationToken)
    {
        var partner = await GetPartnerDetail(partnerId, cancellationToken);

        await InitializeCodebooks(cancellationToken);

        var response = new CustomerDetailResponse
        {
            Identities = { GetIdentities(partner.PartnerId, partner.KbId) },
            NaturalPerson = CreateNaturalPerson(partner),
            IdentificationDocument = partner.ToIdentificationDocument(),
        };

        AddAddress(AddressTypes.Permanent, response.Addresses.Add, partner.Street, partner.HouseNumber, partner.StreetNumber, partner.PostCode, partner.City);
        AddAddress(AddressTypes.Mailing, response.Addresses.Add, partner.MailingStreet, partner.MailingHouseNumber, partner.MailingStreetNumber, partner.MailingPostCode, partner.MailingCity);

        AddContacts(partner, response.Contacts.AddRange);

        return response;
    }

    public async Task<IEnumerable<CustomerDetailResponse>> GetList(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        var partners = await GetPartnerList(partnerIds, cancellationToken);

        if (!partnerIds.Any())
            return Enumerable.Empty<CustomerDetailResponse>();

        await InitializeCodebooks(cancellationToken);

        return partners.Select(p =>
        {
            var detail = new CustomerDetailResponse
            {
                Identities = { GetIdentities(p.PartnerId, p.KbId) },
                NaturalPerson = CreateNaturalPerson(p),
                IdentificationDocument = p.ToIdentificationDocument()
            };

            AddAddress(AddressTypes.Permanent, detail.Addresses.Add, p.Street, p.HouseNumber, p.StreetNumber, p.PostCode, p.City);
            AddAddress(AddressTypes.Mailing, detail.Addresses.Add, p.MailingStreet, p.MailingHouseNumber, p.MailingStreetNumber, p.MailingPostCode, p.MailingCity);

            AddContacts(p, detail.Contacts.AddRange);

            return detail;
        });
    }

    private async Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
    }

    private async Task<Partner> GetPartnerDetail(long partnerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetDetail, parameters: new { partnerId }, cancellationToken: cancellationToken);

        var result = await _connectionProvider.ExecuteDapperQuery(DetailQuery, cancellationToken);

        return result.GroupBy(p => p.PartnerId).Select(p =>
        {
            var partner = p.First();
            partner.Contacts = p.Select(c => c.Contacts.Single()).ToList();

            return partner;
        }).FirstOrDefault() ?? throw new CisNotFoundException(11000, $"Customer ID {partnerId} does not exist.");

        Task<IEnumerable<Partner>> DetailQuery(IDbConnection conn)
        {
            return conn.QueryAsync<Partner, PartnerContact, Partner>(
                command,
                (partner, contact) =>
                {
                    if (contact is not null)
                        partner.Contacts.Add(contact);

                    return partner;
                },
                splitOn: "contactId");
        }
    }

    private async Task<IEnumerable<Partner>> GetPartnerList(IEnumerable<long> partnerIds, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetList, parameters: new { partnerIds }, cancellationToken: cancellationToken);

        var result = await _connectionProvider.ExecuteDapperQuery(ListQuery, cancellationToken);

        return result.GroupBy(p => p.PartnerId).Select(p =>
        {
            var partner = p.First();
            partner.Contacts = p.Select(c => c.Contacts.Single()).ToList();

            return partner;
        });

        Task<IEnumerable<Partner>> ListQuery(IDbConnection conn)
        {
            return conn.QueryAsync<Partner, PartnerContact, Partner>(
                command,
                (partner, contact) =>
                {
                    if (contact is not null)
                        partner.Contacts.Add(contact);

                    return partner;
                },
                splitOn: "contactId");
        }
    }

    private IEnumerable<Identity> GetIdentities(long partnerId, long? kbId)
    {
        yield return new Identity(partnerId, IdentitySchemes.Mp);

        if (kbId.HasValue)
            yield return new Identity(kbId.Value, IdentitySchemes.Kb);
    }

    private NaturalPerson CreateNaturalPerson(Partner partner)
    {
        var person =  new NaturalPerson
        {
            FirstName = partner.FirstName ?? string.Empty,
            LastName = partner.LastName ?? string.Empty,
            GenderId = partner.GenderId,
            DegreeBeforeId = _titles.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeBefore, StringComparison.InvariantCultureIgnoreCase))?.Id,
            BirthNumber = partner.BirthNumber ?? string.Empty,
            DateOfBirth = partner.BirthDate,
            PlaceOfBirth = partner.PlaceOfBirth ?? string.Empty,
            IsPoliticallyExposed = partner.IsPoliticallyExposed
        };

        if (partner.CitizenshipCountryId.HasValue)
            person.CitizenshipCountriesId.Add(partner.CitizenshipCountryId.Value);

        return person;
    }

    private static void AddAddress(AddressTypes addressType, Action<GrpcAddress> onAdd, string? street, string? houseNumber, string? streetNumber, string? postCode, string? city)
    {
        var parameters = new[] { street, houseNumber, streetNumber, postCode, city };

        if (parameters.All(string.IsNullOrWhiteSpace))
            return;

        var address = new GrpcAddress
        {
            AddressTypeId = (int)addressType,
            Street = street ?? string.Empty,
            StreetNumber = streetNumber ?? string.Empty ,
            HouseNumber = houseNumber ?? string.Empty,
            Postcode = postCode ?? string.Empty,
            City = city ?? string.Empty
        };

        onAdd(address);
    }

    private static void AddContacts(Partner partner, Action<IEnumerable<Contact>> onAddContacts)
    {
        var contacts = partner.Contacts.Select(c => new Contact
        {
            IsPrimary = c.IsPrimaryContact,
            ContactTypeId = c.ContactType,
            Value = c.Value ?? string.Empty
        });

        onAddContacts(contacts);
    }
}