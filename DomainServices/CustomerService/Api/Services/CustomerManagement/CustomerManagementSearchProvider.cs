using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using __Contracts = DomainServices.CustomerService.ExternalServices.CustomerManagement.V1.Contracts;
using DomainServices.CustomerService.Api.Extensions;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal class CustomerManagementSearchProvider
{
    private readonly ExternalServices.CustomerManagement.V1.ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceClients _codebook;

    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    public CustomerManagementSearchProvider(ExternalServices.CustomerManagement.V1.ICustomerManagementClient customerManagement, ICodebookServiceClients codebook)
    {
        _customerManagement = customerManagement;
        _codebook = codebook;
    }

    public async Task<IEnumerable<SearchCustomersItem>> Search(SearchCustomersRequest searchRequest, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var foundCustomers = await _customerManagement.Search(ParseRequest(searchRequest), cancellationToken);

        return foundCustomers.Where(c => c.Party is __Contracts.NaturalPersonSearchResult)
                             .Select(c =>
                             {
                                 var item = new SearchCustomersItem
                                 {
                                     Identity = new Identity(c.CustomerId, IdentitySchemes.Kb),
                                     NaturalPerson = CreateNaturalPerson(c),
                                     IdentificationDocument = CreateIdentificationDocument(c.PrimaryIdentificationDocument)
                                 };

                                 FillAddressData(item, c.PrimaryAddress?.Address);

                                 return item;
                             });
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Task.WhenAll(Countries(), Genders(), DocTypes());

        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await _codebook.Genders(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private ExternalServices.CustomerManagement.Dto.CustomerManagementSearchRequest ParseRequest(SearchCustomersRequest searchRequest)
    {
        var cmRequest = new ExternalServices.CustomerManagement.Dto.CustomerManagementSearchRequest
        {
            NumberOfEntries = 20,
            CustomerId = searchRequest.Identity?.IdentityId,
            FirstName = searchRequest.NaturalPerson?.FirstName.ToCMString(),
            Name = searchRequest.NaturalPerson?.LastName.ToCMString(),
            BirthEstablishedDate = searchRequest.NaturalPerson?.DateOfBirth,
            Email = searchRequest.Email.ToCMString(),
            PhoneNumber = searchRequest.PhoneNumber.ToCMString(),
            ShowOnlyIdentified = searchRequest.ShowOnlyIdentified ?? true
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
    }

    private NaturalPersonBasicInfo CreateNaturalPerson(__Contracts.CustomerSearchResultRow customer)
    {
        var np = (__Contracts.NaturalPersonSearchResult)customer.Party;

        return new NaturalPersonBasicInfo
        {
            BirthNumber = np.CzechBirthNumber ?? string.Empty,
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? string.Empty,
            LastName = np.Surname ?? string.Empty,
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id
        };
    }

    private Contracts.IdentificationDocument? CreateIdentificationDocument(__Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return null;

        return new Contracts.IdentificationDocument
        {
            RegisterPlace = document.RegisterPlace ?? string.Empty,
            ValidTo = document.ValidTo,
            IssuedOn = document.IssuedOn,
            IssuedBy = document.IssuedBy ?? string.Empty,
            Number = document.DocumentNumber ?? string.Empty,
            IssuingCountryId = _countries.FirstOrDefault(t => t.ShortName == document.IssuingCountryCode)?.Id,
            IdentificationDocumentTypeId = _docTypes.FirstOrDefault(t => t.RdmCode == document.TypeCode)?.Id ?? 0
        };
    }

    private void FillAddressData(SearchCustomersItem result, __Contracts.Address? address)
    {
        if (address is null)
            return;
        
        result.Street = address.Street ?? string.Empty;
        result.City = address.City ?? string.Empty;
        result.Postcode = address.PostCode ?? string.Empty;
        result.CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id;
    }
}