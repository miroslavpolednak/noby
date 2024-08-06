using DomainServices.CodebookService.Clients;
using CM = DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal sealed class CustomerManagementSearchProvider(
    CM.ICustomerManagementClient _customerManagement, 
    ICodebookServiceClient _codebook)
{
    private List<CountriesResponse.Types.CountryItem> _countries = null!;
    private List<GendersResponse.Types.GenderItem> _genders = null!;
    private List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _docTypes = null!;

    public async Task<IEnumerable<CustomerDetailResponse>> Search(SearchCustomersRequest searchRequest, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var foundCustomers = await _customerManagement.Search(ParseRequest(searchRequest), cancellationToken);

        return foundCustomers.Where(c => c.Party.NaturalPersonAttributes is not null)
                             .Select(c =>
                             {
                                 var item = new CustomerDetailResponse
                                 {
                                     NaturalPerson = new NaturalPerson
                                     {
                                         BirthNumber = c.Party.NaturalPersonAttributes.CzechBirthNumber ?? string.Empty,
                                         DateOfBirth = c.Party.NaturalPersonAttributes.BirthDate,
                                         FirstName = c.Party.NaturalPersonAttributes.FirstName ?? string.Empty,
                                         LastName = c.Party.NaturalPersonAttributes.Surname ?? string.Empty,
                                         GenderId = _genders.FirstOrDefault(t => t.KbCmCode == c.Party.NaturalPersonAttributes.GenderCode)?.Id ?? 0
                                     },
                                     IdentificationDocument = CreateIdentificationDocument(c.PrimaryIdentificationDocument)
                                 };
                                 item.Identities.Add(new SharedTypes.GrpcTypes.Identity(c.CustomerId, IdentitySchemes.Kb));

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

    private IdentificationDocument? CreateIdentificationDocument(CM.Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return default;

        var documentType = _docTypes.FirstOrDefault(t => t.RdmCode == document.TypeCode);

        if (documentType is null)
            return default;

        return new IdentificationDocument
        {
            RegisterPlace = document.RegisterPlace ?? string.Empty,
            ValidTo = document.ValidTo,
            IssuedOn = document.IssuedOn,
            IssuedBy = document.IssuedBy ?? string.Empty,
            Number = document.DocumentNumber ?? string.Empty,
            IssuingCountryId = _countries.FirstOrDefault(t => t.ShortName == document.IssuingCountryCode)?.Id,
            IdentificationDocumentTypeId = documentType.Id
        };
    }

    private void FillAddressData(CustomerDetailResponse result, CM.Contracts.AddressLinePoint? address)
    {
        if (address is null)
            return;

        result.Addresses.Add(new SharedTypes.GrpcTypes.GrpcAddress
        {
            Street = address.Street ?? string.Empty,
            City = address.City ?? string.Empty,
            Postcode = address.PostCode ?? string.Empty,
            CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id
        });
    }
}