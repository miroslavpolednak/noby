﻿using System.Data;
using CIS.Core.Data;
using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.Data;
using CIS.Infrastructure.gRPC.CisTypes;
using Dapper;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb;

[ScopedService, SelfService]
public class KonsDbDetailProvider
{
    private readonly IConnectionProvider _connectionProvider;
    private readonly ICodebookServiceAbstraction _codebook;

    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;

    public KonsDbDetailProvider(IConnectionProvider connectionProvider, ICodebookServiceAbstraction codebook)
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
            Identity = new Identity(partner.PartnerId, IdentitySchemes.Mp),
            NaturalPerson = CreateNaturalPerson(partner),
            IdentificationDocument = CreateIdentificationDocument(partner),
        };

        AddAddress(AddressTypes.PERMANENT, response.Addresses.Add, partner.Street, partner.HouseNumber, partner.StreetNumber, partner.PostCode, partner.City);
        AddAddress(AddressTypes.MAILING, response.Addresses.Add, partner.MailingStreet, partner.MailingHouseNumber, partner.MailingStreetNumber, partner.MailingPostCode, partner.MailingCity);

        AddContacts(partner, response.Contacts.AddRange);

        return response;
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Titles();

        async Task Titles() => _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
    }

    private async Task<Partner> GetPartnerDetail(long partnerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(Sql.SqlScripts.GetDetail, parameters: new { partnerId }, cancellationToken: cancellationToken);

        var result = await _connectionProvider.ExecuteDapperQuery(DetailQuery, cancellationToken);

        return result.FirstOrDefault() ?? throw new CisNotFoundException(11000, $"Customer ID {partnerId} does not exist.");

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

    private NaturalPerson CreateNaturalPerson(Partner partner)
    {
        var person =  new NaturalPerson
        {
            FirstName = partner.FirstName ?? string.Empty,
            LastName = partner.LastName ?? string.Empty,
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

    private IdentificationDocument? CreateIdentificationDocument(Partner partner)
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

    private static void AddAddress(AddressTypes addressType, Action<GrpcAddress> onAdd, string? street, string? houseNumber, string? streetNumber, string? postCode, string? city)
    {
        var parameters = new[] { street, houseNumber, streetNumber, postCode, city };

        if (parameters.All(string.IsNullOrWhiteSpace))
            return;

        var address = new GrpcAddress
        {
            AddressTypeId = (int)addressType,
            Street = street ?? string.Empty,
            BuildingIdentificationNumber = houseNumber ?? string.Empty ,
            LandRegistryNumber = streetNumber ?? string.Empty,
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