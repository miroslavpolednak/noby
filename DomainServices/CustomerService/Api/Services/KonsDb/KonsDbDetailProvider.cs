using System.Data;
using System.Runtime.CompilerServices;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using SharedTypes.Enums;
using CIS.Infrastructure.Data;
using Dapper;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Api.Services.KonsDb.Dto;

namespace DomainServices.CustomerService.Api.Services.KonsDb;

[ScopedService, SelfService]
public class KonsDbDetailProvider
{
    private readonly IMediator _mediator;
    private readonly IConnectionProvider _connectionProvider;
    private readonly ICodebookServiceClient _codebook;

    private List<GenericCodebookResponse.Types.GenericCodebookItem> _titles = null!;

    public KonsDbDetailProvider(IMediator mediator, IConnectionProvider connectionProvider, ICodebookServiceClient codebook)
    {
        _mediator = mediator;
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

        await AddAddress(AddressTypes.Permanent, response.Addresses.Add, partner.Street, partner.HouseNumber, partner.StreetNumber, partner.PostCode, partner.City, partner.CountryId);
        await AddAddress(AddressTypes.Mailing, response.Addresses.Add, partner.MailingStreet, partner.MailingHouseNumber, partner.MailingStreetNumber, partner.MailingPostCode, partner.MailingCity, partner.MailingCountryId);

        AddContacts(partner, response.Contacts.Add);

        return response;
    }

    public async IAsyncEnumerable<CustomerDetailResponse> GetList(IEnumerable<long> partnerIds, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var partners = await GetPartnerList(partnerIds, cancellationToken);

        if (!partnerIds.Any())
            yield break;

        await InitializeCodebooks(cancellationToken);

        foreach (var partner in partners)
        {
            var detail = new CustomerDetailResponse
            {
                Identities = { GetIdentities(partner.PartnerId, partner.KbId) },
                NaturalPerson = CreateNaturalPerson(partner),
                IdentificationDocument = partner.ToIdentificationDocument()
            };

            await AddAddress(AddressTypes.Permanent, detail.Addresses.Add, partner.Street, partner.HouseNumber, partner.StreetNumber, partner.PostCode, partner.City, partner.CountryId);
            await AddAddress(AddressTypes.Mailing, detail.Addresses.Add, partner.MailingStreet, partner.MailingHouseNumber, partner.MailingStreetNumber, partner.MailingPostCode, partner.MailingCity, partner.MailingCountryId);

            AddContacts(partner, detail.Contacts.Add);

            yield return detail;
        }

    }

    private async Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
    }

    private async Task<Partner> GetPartnerDetail(long partnerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetDetail, parameters: new { partnerId }, cancellationToken: cancellationToken);

        var result = await _connectionProvider.ExecuteDapperQueryAsync(DetailQuery, cancellationToken);

        return result.GroupBy(p => p.PartnerId).Select(p =>
        {
            var partner = p.First();
            partner.Contacts = p.SelectMany(c => c.Contacts).ToList();

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

        var result = await _connectionProvider.ExecuteDapperQueryAsync(ListQuery, cancellationToken);

        return result.GroupBy(p => p.PartnerId).Select(p =>
        {
            var partner = p.First();
            partner.Contacts = p.SelectMany(c => c.Contacts).ToList();

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

    private static IEnumerable<Identity> GetIdentities(long partnerId, long? kbId)
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
            DegreeBeforeId = _titles.FirstOrDefault(t => string.Equals(t.Name, partner.DegreeBefore, StringComparison.OrdinalIgnoreCase))?.Id,
            BirthNumber = partner.BirthNumber ?? string.Empty,
            DateOfBirth = partner.BirthDate,
            PlaceOfBirth = partner.PlaceOfBirth ?? string.Empty,
            IsPoliticallyExposed = partner.IsPoliticallyExposed,
            IsUSPerson = partner.IsUSPerson
        };

        if (partner.CitizenshipCountryId.HasValue)
            person.CitizenshipCountriesId.Add(partner.CitizenshipCountryId.Value);

        return person;
    }

    private async Task AddAddress(AddressTypes addressType, Action<GrpcAddress> onAdd, string? street, string? houseNumber, string? streetNumber, string? postCode, string? city, int? countryId)
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
            City = city ?? string.Empty,
            CountryId = countryId is null or 0 ? 16 : countryId //16 = Czech
        };

        if (!string.IsNullOrWhiteSpace(address.City))
        {
            var formatAddressResponse = await _mediator.Send(new FormatAddressRequest { Address = address });

            address.SingleLineAddressPoint = formatAddressResponse.SingleLineAddress;
        }

        onAdd(address);
    }

    // https://jira.kb.cz/browse/HFICH-4238
    private static void AddContacts(Partner partner, Action<Contact> onAddContacts)
    {
        var phone = partner.Contacts.FirstOrDefault(t => t.ContactType == (int)ContactTypes.Mobil);
        if (phone is not null)
            onAddContacts(phone.ToContract());

        var email = partner.Contacts.FirstOrDefault(t => t.ContactType == (int)ContactTypes.Email);
        if (email is not null)
            onAddContacts(email.ToContract());
    }
}