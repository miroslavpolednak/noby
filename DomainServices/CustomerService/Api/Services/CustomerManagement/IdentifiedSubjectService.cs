﻿using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using __Contracts = DomainServices.CustomerService.ExternalServices.IdentifiedSubjectBr.V1.Contracts;
using DomainServices.CustomerService.Api.Extensions;
using FastEnumUtility;
using System.Threading;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal sealed class IdentifiedSubjectService
{
    private readonly ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient _identifiedSubjectClient;
    private readonly ICodebookServiceClients _codebook;
    private readonly CustomerManagementErrorMap _errorMap;
    private readonly ExternalServices.Kyc.V1.IKycClient _kycClient;

    private List<CodebookService.Contracts.Endpoints.Genders.GenderItem> _genders = null!;
    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<CodebookService.Contracts.Endpoints.Countries.CountriesItem> _countries = null!;
    private List<CodebookService.Contracts.Endpoints.MaritalStatuses.MaritalStatusItem> _maritals = null!;
    private List<CodebookService.Contracts.Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    public IdentifiedSubjectService(ExternalServices.IdentifiedSubjectBr.V1.IIdentifiedSubjectBrClient identifiedSubjectClient, ICodebookServiceClients codebook, CustomerManagementErrorMap errorMap, ExternalServices.Kyc.V1.IKycClient kycClient)
    {
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

        return new Identity(_errorMap.ResolveAndThrowIfError(response), IdentitySchemes.Kb);
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

        var identifiedSubject = BuildUpdateRequest(request);

        await _identifiedSubjectClient.UpdateIdentifiedSubject(customerId.IdentityId, identifiedSubject, cancellationToken);

        // https://jira.kb.cz/browse/HFICH-3555
        await callSetSocialCharacteristics(customerId.IdentityId, request, cancellationToken);
        await callSetKyc(customerId.IdentityId, request, cancellationToken);
    }

    private async Task callSetKyc(long customerId, UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = new ExternalServices.Kyc.V1.Contracts.Kyc
        {
            //IsPoliticallyExposed = request.NaturalPerson?.IsPoliticallyExposed ?? false, //!!! nesmi byt zadano, CM pada
            IsUSPerson = request.NaturalPerson?.IsUSPerson ?? false
        };

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
            PrimaryAddress = CreatePrimaryAddress(request.Addresses),
            ContactAddress = CreateContactAddress(request.Addresses),
            PrimaryIdentificationDocument = CreateIdentificationDocument(request.IdentificationDocument),
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
            ValidTo = document.ValidTo,
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