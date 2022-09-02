using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using DomainServices.CustomerService.Contracts;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;

public class KBSearchCustomersParser
{
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    private KBSearchCustomersParser()
    {
    }

    public static async Task<KBSearchCustomersParser> CreateInstance(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        var instance = new KBSearchCustomersParser();

        await instance.Initialize(codebook, cancellationToken);

        return instance;
    }

    public CustomerManagementSearchRequest ParseRequest(SearchCustomersRequest searchRequest)
    {
        var cmRequest = new CustomerManagementSearchRequest
        {
            NumberOfEntries = 20,
            CustomerId = searchRequest.Identity?.IdentityId,
            FirstName = GetValueOrNull(searchRequest.NaturalPerson?.FirstName),
            Name = GetValueOrNull(searchRequest.NaturalPerson?.LastName),
            BirthEstablishedDate = searchRequest.NaturalPerson?.DateOfBirth,
            Email = GetValueOrNull(searchRequest.Email),
            PhoneNumber = GetValueOrNull(searchRequest.PhoneNumber)
        };

        if (!string.IsNullOrEmpty(searchRequest.NaturalPerson?.BirthNumber))
        {
            cmRequest.IdentifierTypeCode = "CZ_RC";
            cmRequest.IdentifierValue = searchRequest.NaturalPerson.BirthNumber;
        }

        if (searchRequest.IdentificationDocument != null)
        {
            cmRequest.IdDocumentTypeCode = _docTypes.First(t => t.Id == searchRequest.IdentificationDocument.IdentificationDocumentTypeId).RdmCode;
            cmRequest.IdDocumentIssuingCountryCode = _countries.First(t => t.Id == searchRequest.IdentificationDocument.IssuingCountryId).ShortName;
            cmRequest.IdDocumentNumber = searchRequest.IdentificationDocument.Number;
        }

        return cmRequest;

        static string? GetValueOrNull(string? str) => string.IsNullOrWhiteSpace(str) ? null : str;
    }

    public SearchCustomersItem ParseResult(CustomerSearchResultRow customer)
    {
        var result = new SearchCustomersItem
        {
            Identity = new Identity(customer.CustomerId, IdentitySchemes.Kb),
            NaturalPerson = CreateNaturalPerson(customer)
        };

        FillAddressData(result, customer.PrimaryAddress.Address);

        return result;
    }

    private Task Initialize(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        return Task.WhenAll(Countries(), Genders(), DocTypes());

        async Task Countries() => _countries = await codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await codebook.Genders(cancellationToken);
        async Task DocTypes() => _docTypes = await codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private NaturalPersonBasicInfo CreateNaturalPerson(CustomerSearchResultRow customer)
    {
        var np = (NaturalPersonSearchResult)customer.Party;

        return new NaturalPersonBasicInfo
        {
            BirthNumber = np.CzechBirthNumber ?? "",
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? "",
            LastName = np.Surname ?? "",
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id
        };
    }

    private void FillAddressData(SearchCustomersItem result, Clients.CustomerManagement.V1.Address? address)
    {
        if (address is null)
            return;

        result.Street = address.Street ?? "";
        result.City = address.City ?? "";
        result.Postcode = address.PostCode ?? "";
        result.CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id;
    }
}