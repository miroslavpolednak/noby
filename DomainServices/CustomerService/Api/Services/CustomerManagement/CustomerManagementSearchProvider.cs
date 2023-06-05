using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using CM = DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal sealed class CustomerManagementSearchProvider
{
    private readonly CM.ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceClient _codebook;

    private List<CountriesResponse.Types.CountryItem> _countries = null!;
    private List<GendersResponse.Types.GenderItem> _genders = null!;
    private List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _docTypes = null!;

    public CustomerManagementSearchProvider(CM.ICustomerManagementClient customerManagement, ICodebookServiceClient codebook)
    {
        _customerManagement = customerManagement;
        _codebook = codebook;
    }

    public async Task<IEnumerable<SearchCustomersItem>> Search(SearchCustomersRequest searchRequest, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var foundCustomers = await _customerManagement.Search(ParseRequest(searchRequest), cancellationToken);

        return foundCustomers.Where(c => c.Party.NaturalPersonAttributes is not null)
                             .Select(c =>
                             {
                                 var item = new SearchCustomersItem
                                 {
                                     Identity = new Identity(c.CustomerId, IdentitySchemes.Kb),
                                     NaturalPerson = CreateNaturalPerson(c.Party.NaturalPersonAttributes),
                                     IdentificationDocument = CreateIdentificationDocument(c.PrimaryIdentificationDocument)
                                 };

                                 FillAddressData(item, c.PrimaryAddress?.AddressLinePoint);

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
            Email = $"{searchRequest.Email?.EmailAddress}".ToCMString(),
            PhoneNumber = $"{searchRequest.MobilePhone?.PhoneIDC}{searchRequest.MobilePhone?.PhoneNumber}".ToCMString(),
            ShowOnlyIdentified = searchRequest.SearchOnlyIdentified ?? true
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

    private NaturalPersonBasicInfo CreateNaturalPerson(CM.Contracts.NaturalPersonAttributesSearch customer)
    {
        return new NaturalPersonBasicInfo
        {
            BirthNumber = customer.CzechBirthNumber ?? string.Empty,
            DateOfBirth = customer.BirthDate,
            FirstName = customer.FirstName ?? string.Empty,
            LastName = customer.Surname ?? string.Empty,
            GenderId = _genders.FirstOrDefault(t => t.KbCmCode == customer.GenderCode)?.Id ?? 0
        };
    }

    private IdentificationDocument? CreateIdentificationDocument(CM.Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return null;

        return new IdentificationDocument
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

    private void FillAddressData(SearchCustomersItem result, CM.Contracts.AddressLinePoint? address)
    {
        if (address is null)
            return;
        
        result.Street = address.Street ?? string.Empty;
        result.City = address.City ?? string.Empty;
        result.Postcode = address.PostCode ?? string.Empty;
        result.CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id;
    }
}