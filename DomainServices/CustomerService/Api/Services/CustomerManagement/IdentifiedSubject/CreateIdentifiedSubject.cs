﻿using System.Diagnostics;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1;
using DomainServices.CustomerService.Api.Extensions;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;
using IdentificationDocument = DomainServices.CustomerService.Api.Clients.IdentifiedSubjectBr.V1.IdentificationDocument;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal class CreateIdentifiedSubject
{
    private readonly IIdentifiedSubjectClient _identifiedSubjectClient;
    private readonly ICodebookServiceAbstraction _codebook;
    private readonly CreateIdentifiedSubjectErrorMap _errorMap;

    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.MaritalStatuses.MaritalStatusItem> _maritals = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    public CreateIdentifiedSubject(IIdentifiedSubjectClient identifiedSubjectClient, ICodebookServiceAbstraction codebook, CreateIdentifiedSubjectErrorMap errorMap)
    {
        _identifiedSubjectClient = identifiedSubjectClient;
        _codebook = codebook;
        _errorMap = errorMap;
    }

    public async Task<Identity> CreateSubject(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await InitializeCodebooks(cancellationToken);

        var createRequest = BuildCreateRequest(request);

        var response = await _identifiedSubjectClient.CreateIdentifiedSubject(createRequest, request.HardCreate, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

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

    private IdentifiedSubject BuildCreateRequest(CreateCustomerRequest request)
    {
        return new IdentifiedSubject
        {
            Party = new Party
            {
                LegalStatus = PartyLegalStatus.P,
                NaturalPersonAttributes = CreateNaturalPersonAttributes(request.NaturalPerson)
            },
            PrimaryAddress = CreatePrimaryAddress(request.Addresses),
            ContactAddress = CreateContactAddress(request.Addresses),
            PrimaryIdentificationDocument = CreateIdentificationDocument(request.IdentificationDocument),
            PrimaryPhone = CreatePrimaryPhone(request.Contacts),
            PrimaryEmail = CreatePrimaryEmail(request.Contacts)
        };
    }

    private NaturalPersonAttributes CreateNaturalPersonAttributes(NaturalPerson naturalPerson)
    {
        return new NaturalPersonAttributes
        {
            FirstName = naturalPerson.FirstName,
            Surname = naturalPerson.LastName,
            GenderCode = Enum.Parse<NaturalPersonAttributesGenderCode>(_genders.First(g => g.Id == naturalPerson.GenderId).KbCmCode, true),
            BirthDate = naturalPerson.DateOfBirth,
            Title = _titles.FirstOrDefault(t => t.Id == naturalPerson.DegreeBeforeId)?.Name,
            CzechBirthNumber = naturalPerson.BirthNumber.ToCMString(),
            CitizenshipCodes = naturalPerson.CitizenshipCountriesId.Select(id => _countries.First(c => c.Id == id).ShortName).ToList(),
            BirthCountryCode = _countries.FirstOrDefault(c => c.Id == naturalPerson.BirthCountryId)?.ShortName,
            MaritalStatusCode = _maritals.First(m => m.Id == naturalPerson.MaritalStatusStateId).RdmMaritalStatusCode,
            BirthPlace = naturalPerson.PlaceOfBirth.ToCMString(),
            BirthName = naturalPerson.BirthName.ToCMString()
        };
    }

    private PrimaryAddress? CreatePrimaryAddress(IEnumerable<GrpcAddress> addresses)
    {
        var primaryAddress = addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.PERMANENT);

        if (primaryAddress is null)
            return default;

        return new PrimaryAddress
        {
            Address = CreateAddress(primaryAddress)
        };
    }

    private ContactAddress? CreateContactAddress(IEnumerable<GrpcAddress> addresses)
    {
        var contactAddress = addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.MAILING);

        if (contactAddress is null)
            return default;

        return new ContactAddress
        {
            Confirmed = true,
            Address = CreateAddress(contactAddress)
        };
    }

    private Address CreateAddress(GrpcAddress address)
    {
        return new Address
        {
            City = address.City,
            PostCode = address.Postcode.ToCMString(),
            CountryCode = _countries.FirstOrDefault(c => c.Id == address.CountryId)?.ShortName,
            Street = address.Street.ToCMString(),
            HouseNumber = address.BuildingIdentificationNumber.ToCMString(),
            StreetNumber = address.LandRegistryNumber.ToCMString(),
            DeliveryDetails = address.DeliveryDetails.ToCMString()
        };
    }

    private IdentificationDocument? CreateIdentificationDocument(Contracts.IdentificationDocument? document)
    {
        if (document is null)
            return default;

        return new IdentificationDocument
        {
            DocumentNumber = document.Number,
            TypeCode = _docTypes.First(d => d.Id == document.IdentificationDocumentTypeId).RdmCode,
            IssuedBy = document.IssuedBy.ToCMString(),
            IssuingCountryCode = _countries.FirstOrDefault(c => c.Id == document.IssuingCountryId)?.ShortName,
            IssuedOn = document.IssuedOn,
            ValidTo = document.ValidTo
        };
    }

    private static PrimaryPhone? CreatePrimaryPhone(IEnumerable<Contact> contacts)
    {
        var phone = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.MobilPrivate);

        if (phone is null)
            return default;

        return new PrimaryPhone
        {
            PhoneNumber = phone.Value
        };
    }

    private static PrimaryEmail? CreatePrimaryEmail(IEnumerable<Contact> contacts)
    {
        var email = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        if (email is null)
            return default;

        return new PrimaryEmail
        {
            EmailAddress = email.Value
        };
    }
}