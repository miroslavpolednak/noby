using System.Globalization;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Countries;
using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;
using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.LoanApplication;

internal class LoanApplicationCustomer
{
    private readonly CustomerDetailResponse _customer;

    private readonly List<GenericCodebookItem> _degreesBefore;
    private readonly List<CountriesItem> _countries;
    private readonly List<IdentificationDocumentTypesItem> _identificationDocumentTypes;
    private readonly List<EducationLevelItem> _educationLevels;

    public LoanApplicationCustomer(CustomerDetailResponse customer,
                                   List<GenericCodebookItem> degreesBefore,
                                   List<CountriesItem> countries,
                                   List<IdentificationDocumentTypesItem> identificationDocumentTypes,
                                   List<EducationLevelItem> educationLevels)
    {
        _customer = customer;
        _degreesBefore = degreesBefore;
        _countries = countries;
        _identificationDocumentTypes = identificationDocumentTypes;
        _educationLevels = educationLevels;
    }

    public string FullName => GetFullName();

    public string PermanentAddress => FormatAddress(_customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent));

    public string ContactAddress => GetContactAddress();

    public NullableGrpcDate DateOfBirth => _customer.NaturalPerson.DateOfBirth;

    public string? BirthNumber => string.IsNullOrWhiteSpace(_customer.NaturalPerson.BirthNumber) ? null : _customer.NaturalPerson.BirthNumber;

    public string IdentificationType => GetIdentificationDocument();

    public string Contacts => GetContacts();

    public int MaritalStatusStateId => _customer.NaturalPerson.MaritalStatusStateId;

    public string EducationLevel => GetEducationLevel();

    public NaturalPersonResidenceCountry? CzechResidence => _customer.NaturalPerson.TaxResidence?.ResidenceCountries.FirstOrDefault(r => r.CountryId == 16);

    private string GetFullName()
    {
        if (!_customer.NaturalPerson.DegreeBeforeId.HasValue)
            return $"{_customer.NaturalPerson.FirstName} {_customer.NaturalPerson.LastName}";

        var degree = _degreesBefore.First(d => d.Id == _customer.NaturalPerson.DegreeBeforeId.Value).Name;

        return $"{_customer.NaturalPerson.FirstName} {_customer.NaturalPerson.LastName}, {degree}";
    }

    private string FormatAddress(GrpcAddress? address)
    {
        if (address is null)
            return string.Empty;

        var countryName = _countries.First(c => c.Id == address.CountryId).LongName;

        return $"{address.Street} {address.HouseNumber}/{address.StreetNumber}, {address.Postcode} {address.City}, {countryName}";
    }

    private string GetContactAddress()
    {
        var contactAddress = _customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Mailing);

        if (contactAddress is not null)
            return FormatAddress(contactAddress);

        if (_customer.Addresses.Any(a => a.AddressTypeId == (int)AddressTypes.Permanent) || _customer.NaturalPerson.CitizenshipCountriesId.Any(id => id == 16))
            return PermanentAddress;

        var otherAddress = _customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Other);
        return FormatAddress(otherAddress);
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