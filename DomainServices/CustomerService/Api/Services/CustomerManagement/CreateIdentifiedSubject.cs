using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Api.Clients;
using __Contracts = DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;
using DomainServices.CustomerService.Api.Extensions;
using FastEnumUtility;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal class CreateIdentifiedSubject
{
    private readonly ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient _identifiedSubjectClient;
    private readonly ICodebookServiceClients _codebook;
    private readonly CustomerManagementErrorMap _errorMap;

    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.MaritalStatuses.MaritalStatusItem> _maritals = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    public CreateIdentifiedSubject(ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient identifiedSubjectClient, ICodebookServiceClients codebook, CustomerManagementErrorMap errorMap)
    {
        _identifiedSubjectClient = identifiedSubjectClient;
        _codebook = codebook;
        _errorMap = errorMap;
    }

    public async Task<Identity> CreateSubject(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var createRequest = BuildCreateRequest(request);

        var response = await _identifiedSubjectClient.CreateIdentifiedSubject(createRequest, request.HardCreate, cancellationToken);

        return new Identity(_errorMap.ResolveAndThrowIfError(response), IdentitySchemes.Kb);
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Task.WhenAll(Genders(), Titles(), Countries(), Maritals(), DocTypes());

        async Task Genders() => _genders = await _codebook.Genders(cancellationToken);
        async Task Titles() => _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task Maritals() => _maritals = await _codebook.MaritalStatuses(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private __Contracts.IdentifiedSubject BuildCreateRequest(CreateCustomerRequest request)
    {
        return new()
        {
            Party = new __Contracts.Party
            {
                LegalStatus = __Contracts.PartyLegalStatus.P,
                NaturalPersonAttributes = CreateNaturalPersonAttributes(request.NaturalPerson)
            },
            PrimaryAddress = CreatePrimaryAddress(request.Addresses),
            ContactAddress = CreateContactAddress(request.Addresses),
            PrimaryIdentificationDocument = CreateIdentificationDocument(request.IdentificationDocument),
            PrimaryPhone = CreatePrimaryPhone(request.Contacts),
            PrimaryEmail = CreatePrimaryEmail(request.Contacts)
        };
    }

    private __Contracts.NaturalPersonAttributes CreateNaturalPersonAttributes(NaturalPerson naturalPerson)
    {
        var citizenshipCodes = naturalPerson.CitizenshipCountriesId.Select(id => _countries.First(c => c.Id == id).ShortName).ToList();

        return new()
        {
            FirstName = naturalPerson.FirstName,
            Surname = naturalPerson.LastName,
            GenderCode = FastEnum.Parse<__Contracts.NaturalPersonAttributesGenderCode>(_genders.First(g => g.Id == naturalPerson.GenderId).KbCmCode, true),
            BirthDate = naturalPerson.DateOfBirth,
            Title = _titles.FirstOrDefault(t => t.Id == naturalPerson.DegreeBeforeId)?.Name,
            CzechBirthNumber = naturalPerson.BirthNumber.ToCMString(),
            CitizenshipCodes = citizenshipCodes.Any() ? citizenshipCodes : null,
            BirthCountryCode = _countries.FirstOrDefault(c => c.Id == naturalPerson.BirthCountryId)?.ShortName,
            MaritalStatusCode = _maritals.First(m => m.Id == naturalPerson.MaritalStatusStateId).RdmMaritalStatusCode,
            BirthPlace = naturalPerson.PlaceOfBirth.ToCMString(),
            BirthName = naturalPerson.BirthName.ToCMString()
        };
    }

    private __Contracts.PrimaryAddress? CreatePrimaryAddress(IEnumerable<GrpcAddress> addresses)
    {
        var primaryAddress = addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent);

        if (primaryAddress is null)
            return default;

        return new()
        {
            Address = CreateAddress(primaryAddress),
            PrimaryAddressFrom = primaryAddress.PrimaryAddressFrom
        };
    }

    private __Contracts.ContactAddress? CreateContactAddress(IEnumerable<GrpcAddress> addresses)
    {
        var contactAddress = addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Mailing);

        if (contactAddress is null)
            return default;

        return new()
        {
            Confirmed = true,
            Address = CreateAddress(contactAddress)
        };
    }

    private __Contracts.Address CreateAddress(GrpcAddress address)
    {
        return new()
        {
            City = address.City,
            PostCode = address.Postcode.ToCMString(),
            CountryCode = _countries.FirstOrDefault(c => c.Id == address.CountryId)?.ShortName,
            Street = address.Street.ToCMString(),
            HouseNumber = address.HouseNumber.ToCMString(),
            StreetNumber = address.StreetNumber.ToCMString(),
            EvidenceNumber = address.EvidenceNumber.ToCMString(),
            DeliveryDetails = address.DeliveryDetails.ToCMString(),
            CityDistrict = address.CityDistrict.ToCMString(),
            PragueDistrict = address.PragueDistrict.ToCMString(),
            CountrySubdivision = address.CountrySubdivision.ToCMString(),
            AddressPointId = address.AddressPointId.ToCMString()
        };
    }

    private __Contracts.IdentificationDocument? CreateIdentificationDocument(Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return default;

        return new()
        {
            DocumentNumber = document.Number,
            TypeCode = _docTypes.First(d => d.Id == document.IdentificationDocumentTypeId).RdmCode,
            IssuedBy = document.IssuedBy.ToCMString(),
            IssuingCountryCode = _countries.FirstOrDefault(c => c.Id == document.IssuingCountryId)?.ShortName,
            IssuedOn = document.IssuedOn,
            ValidTo = document.ValidTo
        };
    }

    private static __Contracts.PrimaryPhone? CreatePrimaryPhone(IEnumerable<Contact> contacts)
    {
        var phone = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);

        if (phone is null || string.IsNullOrWhiteSpace(phone.Value))
            return default;

        var phoneNumber = phone.Value.Replace(" ", "");

        var phoneIDC = phoneNumber[..Math.Max(0, phoneNumber.Length - 9)];
        phoneNumber = phoneNumber.Substring(phoneIDC.Length, phoneNumber.Length - phoneIDC.Length);

        return new()
        {
            PhoneIDC = phoneIDC,
            PhoneNumber = phoneNumber
        };
    }

    private static __Contracts.PrimaryEmail? CreatePrimaryEmail(IEnumerable<Contact> contacts)
    {
        var email = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        if (email is null)
            return default;

        return new()
        {
            EmailAddress = email.Value
        };
    }
}