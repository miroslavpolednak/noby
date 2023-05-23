using System.Globalization;
using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using __Contracts = DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;
using DomainServices.CustomerService.Api.Extensions;
using FastEnumUtility;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal sealed class IdentifiedSubjectService
{
    private readonly ExternalServices.CustomerManagement.V2.ICustomerManagementClient _customerManagement;
    private readonly ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient _identifiedSubjectClient;
    private readonly ICodebookServiceClient _codebook;
    private readonly CustomerManagementErrorMap _errorMap;
    private readonly ExternalServices.Kyc.V1.IKycClient _kycClient;

    private List<GendersResponse.Types.GenderItem> _genders = null!;
    private List<GenericCodebookResponse.Types.GenericCodebookItem> _titles = null!;
    private List<CountriesResponse.Types.CountryItem> _countries = null!;
    private List<MaritalStatusesResponse.Types.MaritalStatuseItem> _maritals = null!;
    private List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _docTypes = null!;

    public IdentifiedSubjectService(ExternalServices.CustomerManagement.V2.ICustomerManagementClient customerManagement, ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient identifiedSubjectClient, ICodebookServiceClient codebook, CustomerManagementErrorMap errorMap, ExternalServices.Kyc.V1.IKycClient kycClient)
    {
        _customerManagement = customerManagement;
        _identifiedSubjectClient = identifiedSubjectClient;
        _codebook = codebook;
        _errorMap = errorMap;
        _kycClient = kycClient;
    }

    public async Task<Identity> CreateSubject(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var identifiedSubject = BuildCreateRequest(request);

        var response = await _identifiedSubjectClient.CreateIdentifiedSubject(identifiedSubject, request.HardCreate, cancellationToken);

        if (response.Error is not null) 
            _errorMap.ResolveValidationError(response.Error);

        return new Identity(CustomerManagementErrorMap.ResolveAndThrowIfError(response.Result!), IdentitySchemes.Kb);
    }

    public async Task UpdateSubject(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var customerId = request.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        if (customerId is null)
        {
            // todo: error_code
            throw new CisArgumentException(9999999, "Customer does not have KB Identity", "IdentityId");
        }

        var customer = await _customerManagement.GetDetail(customerId.IdentityId, cancellationToken);

        var identifiedSubject = BuildUpdateRequest(request);

        await _identifiedSubjectClient.UpdateIdentifiedSubject(customerId.IdentityId, identifiedSubject, cancellationToken);
        
        // https://jira.kb.cz/browse/HFICH-3555
        await callSetSocialCharacteristics(customerId.IdentityId, request, cancellationToken);
        await callSetKyc(customerId.IdentityId, request, customer.IsPoliticallyExposed, cancellationToken);
    }

    private async Task callSetKyc(long customerId, UpdateCustomerRequest request, bool? isPoliticallyExposed, CancellationToken cancellationToken)
    {
        var model = new ExternalServices.Kyc.V1.Contracts.Kyc
        {
            IsUSPerson = request.NaturalPerson?.IsUSPerson ?? false
        };

        // jedine v tomto pripade se muze do CM poslat neco v IsPoliticallyExposed
        if (!isPoliticallyExposed.HasValue && (request.NaturalPerson?.IsUSPerson ?? false))
        {
            model.IsPoliticallyExposed = false;
        }

        if (request.NaturalPerson?.TaxResidence is not null)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            model.TaxResidence = new ExternalServices.Kyc.V1.Contracts.TaxResidence
            {
                ResidenceCountries = request.NaturalPerson?.TaxResidence?.ResidenceCountries?.Select(x => new ExternalServices.Kyc.V1.Contracts.TaxResidenceCountry
                {
                    Tin = x.Tin,
                    TinMissingReasonDescription = x.TinMissingReasonDescription,
                    CountryCode = _countries.FirstOrDefault(c => c.Id == x.CountryId)?.ShortName
                }).ToList(),
                ValidFrom = request.NaturalPerson!.TaxResidence.ValidFrom
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        await _kycClient.SetKyc(customerId, model, cancellationToken);
    }

    private async Task callSetSocialCharacteristics(long customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        //! Honza rika, ze neni treba posilat nic jineho nez Education
        var model = new ExternalServices.Kyc.V1.Contracts.SocialCharacteristics
        {
            Education = new()
            {
                Code = (await _codebook.EducationLevels(cancellationToken)).FirstOrDefault(t => t.Id == request.NaturalPerson?.EducationLevelId)?.RdmCode ?? ""
            }/*,
            MaritalStatus = new()
            {
                Code = _maritals.FirstOrDefault(t => t.Id == request.NaturalPerson?.MaritalStatusStateId)?.RdmMaritalStatusCode ?? ""
            }*/
        };

        await _kycClient.SetSocialCharacteristics(customerId, model, cancellationToken);
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
            PrimaryAddress = CreateAddress(request.Addresses, AddressTypes.Permanent, CreatePrimaryAddress),
            ContactAddress = CreateAddress(request.Addresses, AddressTypes.Mailing, CreateContactAddress),
            TemporaryStay = CreateAddress(request.Addresses, AddressTypes.Other, CreateTemporaryStayAddress),
            PrimaryIdentificationDocument = CreateIdentificationDocument(request.IdentificationDocument),
            CustomerIdentification = CreateCustomerIdentification(request.CustomerIdentification),
            PrimaryPhone = CreatePrimaryPhone(request.Contacts),
            PrimaryEmail = CreatePrimaryEmail(request.Contacts)
        };
    }

    private __Contracts.IdentifiedSubject BuildUpdateRequest(UpdateCustomerRequest request)
    {
        return new()
        {
            Party = new __Contracts.Party
            {
                LegalStatus = __Contracts.PartyLegalStatus.P,
                NaturalPersonAttributes = CreateNaturalPersonAttributes(request.NaturalPerson)
            },
            PrimaryAddress = CreateAddress(request.Addresses, AddressTypes.Permanent, CreatePrimaryAddress),
            ContactAddress = CreateAddress(request.Addresses, AddressTypes.Mailing, CreateContactAddress),
            TemporaryStay = CreateAddress(request.Addresses, AddressTypes.Other, CreateTemporaryStayAddress),
            PrimaryIdentificationDocument = CreateIdentificationDocument(request.IdentificationDocument),
            CustomerIdentification = CreateCustomerIdentification(request.CustomerIdentification),
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

    private TAddress? CreateAddress<TAddress>(IEnumerable<GrpcAddress> addresses, AddressTypes addressType, Func<GrpcAddress, __Contracts.Address, DateTime?, TAddress> factory)
    {
        var address = addresses.FirstOrDefault(a => a.AddressTypeId == (int)addressType);

        if (address is null)
            return default;

        var parsedAddress = new __Contracts.Address
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

        return factory(address, parsedAddress, address.PrimaryAddressFrom);
    }

    private static __Contracts.PrimaryAddress CreatePrimaryAddress(GrpcAddress requestAddress, __Contracts.Address address, DateTime? primaryAddressFrom) =>
        new()
        {
            Address = address,
            PrimaryAddressFrom = primaryAddressFrom
        };  
    
    private static __Contracts.ContactAddress CreateContactAddress(GrpcAddress requestAddress, __Contracts.Address address, DateTime? primaryAddressFrom) =>
        new()
        {
            Address = address,
            Confirmed = requestAddress.IsAddressConfirmed ?? false
        };

    private static __Contracts.TemporaryStayAddress CreateTemporaryStayAddress(GrpcAddress requestAddress, __Contracts.Address address, DateTime? primaryAddressFrom) =>
        new() { Address = address };

    private __Contracts.IdentificationDocument? CreateIdentificationDocument(IdentificationDocument? document)
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
            ValidTo = document.ValidTo,
        };
    }

    private static __Contracts.CustomerIdentification? CreateCustomerIdentification(CustomerIdentification? customerIdentification)
    {
        if (customerIdentification is null)
            return default;

        return new __Contracts.CustomerIdentification
        {
            IdentificationMethodCode = customerIdentification.IdentificationMethodId.ToString(CultureInfo.InvariantCulture),
            IdentificationDate = customerIdentification.IdentificationDate,
            CzechIdentificationNumber = customerIdentification.CzechIdentificationNumber.ToCMString()
        };
    }

    private static __Contracts.PrimaryPhone? CreatePrimaryPhone(IEnumerable<Contact> contacts)
    {
        var phone = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);
        if (string.IsNullOrWhiteSpace(phone?.Mobile?.PhoneNumber))
            return default;

        return new()
        {
            PhoneIDC = phone.Mobile.PhoneIDC,
            PhoneNumber = phone.Mobile.PhoneNumber,
            Confirmed = phone.Mobile.IsPhoneConfirmed
        };
    }

    private static __Contracts.PrimaryEmail? CreatePrimaryEmail(IEnumerable<Contact> contacts)
    {
        var email = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        if (email is null)
            return default;

        return new()
        {
            EmailAddress = email.Email.EmailAddress,
            Confirmed = email.Email.IsEmailConfirmed
        };
    }
}