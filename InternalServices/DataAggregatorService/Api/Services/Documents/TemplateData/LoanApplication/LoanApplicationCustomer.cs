using System.Globalization;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal class LoanApplicationCustomer
{
    private readonly CustomerDetailResponse _customer;

    private readonly List<GenericCodebookResponse.Types.GenericCodebookItem> _degreesBefore;
    private readonly List<CountriesResponse.Types.CountryItem> _countries;
    private readonly List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _identificationDocumentTypes;
    private readonly List<EducationLevelsResponse.Types.EducationLevelItem> _educationLevels;

    public LoanApplicationCustomer(CustomerDetailResponse customer,
                                   List<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore,
                                   List<CountriesResponse.Types.CountryItem> countries,
                                   List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> identificationDocumentTypes,
                                   List<EducationLevelsResponse.Types.EducationLevelItem> educationLevels)
    {
        _customer = customer;
        _degreesBefore = degreesBefore;
        _countries = countries;
        _identificationDocumentTypes = identificationDocumentTypes;
        _educationLevels = educationLevels;
    }

    public string FullName => CustomerHelper.FullName(_customer, _degreesBefore);

    public string SignerName => CustomerHelper.FullName(_customer);

    public string PermanentAddress => CustomerHelper.FullAddress(_customer, AddressTypes.Permanent, _countries);

    public string ContactAddress => GetContactAddress();

    public NullableGrpcDate DateOfBirth => _customer.NaturalPerson.DateOfBirth;

    public string? BirthNumber => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? null : _customer.NaturalPerson.BirthNumber;

    public string IdentificationType => GetIdentificationDocument();

    public string Contacts => GetContacts();

    public int MaritalStatusStateId => _customer.NaturalPerson.MaritalStatusStateId;

    public string EducationLevel => GetEducationLevel();

    public NaturalPersonResidenceCountry? CzechResidence => _customer.NaturalPerson.TaxResidence?.ResidenceCountries.FirstOrDefault(r => r.CountryId == 16);

    private string GetContactAddress()
    {
        var contactAddress = _customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Mailing);

        if (contactAddress is not null)
            return CustomerHelper.FullAddress(contactAddress, _countries);

        if (_customer.Addresses.Any(a => a.AddressTypeId == (int)AddressTypes.Permanent))
            return PermanentAddress;

        return CustomerHelper.FullAddress(_customer, AddressTypes.Other, _countries);
    }

    private string GetIdentificationDocument()
    {
        var document = _customer.IdentificationDocument;

        if (document is null)
            return string.Empty;

        var documentType = _identificationDocumentTypes.First(x => x.Id == document.IdentificationDocumentTypeId).Name;
        var countryName = _countries.First(c => c.Id == document.IssuingCountryId).LongName;

        var validToText = document.ValidTo is null ? "n/a" : ((DateTime)document.ValidTo).ToString("d", CultureInfo.GetCultureInfo("cs"));

        return $"{documentType} č.: {document.Number}, platný do: {validToText}, vydal: {document.IssuedBy}, {countryName}";
    }

    private string GetContacts()
    {
        var email = _customer.Contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);
        var phone = _customer.Contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);

        if (email is not null && phone is not null)
            return $"{phone.Mobile?.PhoneIDC} {phone.Mobile?.PhoneNumber} | {email.Email?.EmailAddress}";

        if (email is not null)
            return $"{email.Email?.EmailAddress}";

        if (phone is not null)
            return $"{phone.Mobile?.PhoneIDC} {phone.Mobile?.PhoneNumber}";

        return string.Empty;
    }

    private string GetEducationLevel()
    {
        return _educationLevels.Where(e => e.Id == _customer.NaturalPerson.EducationLevelId)
                               .Select(e => e.ShortName)
                               .DefaultIfEmpty(string.Empty)
                               .First();
    }
}