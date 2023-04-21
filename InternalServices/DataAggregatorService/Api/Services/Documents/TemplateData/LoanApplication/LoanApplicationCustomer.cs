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

    public string ContactAddress => FormatAddress(_customer.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Mailing));

    public NullableGrpcDate DateOfBirth => _customer.NaturalPerson.DateOfBirth;

    public string? BirthNumber => _customer.NaturalPerson.BirthNumber;

    public string IdentificationType => GetIdentificationDocument();

    public string Contacts => GetContacts();

    public int MaritalStatusStateId => _customer.NaturalPerson.MaritalStatusStateId;

    public string EducationLevel => GetEducationLevel();

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

    private string GetIdentificationDocument()
    {
        var document = _customer.IdentificationDocument;

        var documentType = _identificationDocumentTypes.First(x => x.Id == document.IdentificationDocumentTypeId).Name;
        var countryName = _countries.First(c => c.Id == document.IssuingCountryId).LongName;

        return $"{documentType} č.: {document.Number}, platný do: {(DateTime?)document.ValidTo}, vydal: {document.IssuedBy}, {countryName}";
    }

    private string GetContacts()
    {
        var email = _customer.Contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);
        var phone = _customer.Contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);

        if (email is not null && phone is not null)
            return $"telefon: {phone.Mobile?.PhoneIDC}{phone.Mobile?.PhoneNumber} | e-mail: {email.Email?.EmailAddress}";

        if (email is not null)
            return $"e-mail: {email.Email?.EmailAddress}";

        if (phone is not null)
            return $"telefon: {phone.Mobile?.PhoneIDC}{phone.Mobile?.PhoneNumber}";

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