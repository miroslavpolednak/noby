﻿using System.Diagnostics;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal class CustomerManagementDetailProvider
{
    private readonly ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceClients _codebook;

    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.MaritalStatuses.MaritalStatusItem> _maritals = null!;
    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<Endpoints.EducationLevels.EducationLevelItem> _educations = null!;
    private List<Endpoints.ProfessionTypes.ProfessionTypeItem> _professionTypes = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;
    private List<Endpoints.NetMonthEarnings.NetMonthEarningItem> _netMonthEarnings = null!;
    private List<Endpoints.LegalCapacityRestrictionTypes.LegalCapacityRestrictionTypeItem> _legalCapacityRestrictionTypes = null!;
    private List<CodebookService.Contracts.GenericCodebookItemWithRdmCode> _incomeMainTypesAML = null!;

    public CustomerManagementDetailProvider(ICustomerManagementClient customerManagement, ICodebookServiceClients codebook)
    {
        _customerManagement = customerManagement;
        _codebook = codebook;
    }

    public async Task<CustomerDetailResponse> GetDetail(long customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerManagement.GetDetail(customerId, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

        await InitializeCodebooks(cancellationToken);

        return CreateDetailResponse(customer);
    }

    public async Task<IEnumerable<CustomerDetailResponse>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        var customers = await _customerManagement.GetList(customerIds, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

        if (!customers.Any())
            return Enumerable.Empty<CustomerDetailResponse>();

        await InitializeCodebooks(cancellationToken);

        return customers.Select(CreateDetailResponse);
    }

    private CustomerDetailResponse CreateDetailResponse(CustomerBaseInfo customer)
    {
        var response = new CustomerDetailResponse
        {
            Identities = { new Identity(customer.CustomerId, IdentitySchemes.Kb) },
            NaturalPerson = CreateNaturalPerson(customer),
            IdentificationDocument = CreateIdentificationDocument(customer.PrimaryIdentificationDocument)
        };

        AddAddress(AddressTypes.Permanent, response.Addresses.Add, customer.PrimaryAddress?.Address, customer.PrimaryAddress?.ComponentAddress, customer.PrimaryAddress?.PrimaryAddressFrom);
        AddAddress(AddressTypes.Mailing, response.Addresses.Add, customer.ContactAddress?.Address, customer.ContactAddress?.ComponentAddress, default);

        AddContacts(customer, response.Contacts.Add);

        return response;
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Task.WhenAll(Countries(), Genders(), Maritals(), Titles(), Educations(), DocTypes(), ProfessionTypes(), NetMonthEarnings(), IncomeMainTypesAML());

        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await _codebook.Genders(cancellationToken);
        async Task Maritals() => _maritals = await _codebook.MaritalStatuses(cancellationToken);
        async Task Titles() => _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
        async Task Educations() => _educations = await _codebook.EducationLevels(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
        async Task ProfessionTypes() => _professionTypes = await _codebook.ProfessionTypes(cancellationToken);
        async Task NetMonthEarnings() => _netMonthEarnings = await _codebook.NetMonthEarnings(cancellationToken);
        async Task IncomeMainTypesAML() => _incomeMainTypesAML = await _codebook.IncomeMainTypesAML(cancellationToken);
    }

    private Contracts.NaturalPerson CreateNaturalPerson(CustomerBaseInfo customer)
    {
        var np = (Clients.CustomerManagement.V1.NaturalPerson)customer.Party;

        var person = new Contracts.NaturalPerson
        {
            BirthNumber = np.CzechBirthNumber ?? string.Empty,
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? string.Empty,
            LastName = np.Surname ?? string.Empty,
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id,
            BirthName = np.BirthName ?? string.Empty,
            PlaceOfBirth = np.BirthPlace ?? string.Empty,
            BirthCountryId = _countries.FirstOrDefault(t => t.ShortName == np.BirthCountryCode)?.Id,
            MaritalStatusStateId = _maritals.FirstOrDefault(t => t.RdmMaritalStatusCode == np.MaritalStatusCode)?.Id ?? 0,
            DegreeBeforeId = _titles.FirstOrDefault(t => string.Equals(t.Name, np.Title, StringComparison.InvariantCultureIgnoreCase))?.Id,
            EducationLevelId = _educations.FirstOrDefault(t => t.RdmCode.Equals(customer.Kyc?.NaturalPersonKyc?.EducationCode ?? "", StringComparison.InvariantCultureIgnoreCase))?.Id ?? 0,
            IsPoliticallyExposed = customer.IsPoliticallyExposed,
            IsUSPerson = false, //je vzdy false!
            IsBrSubscribed = customer.BrSubscription?.IsSubscribed ?? false,
            KbRelationshipCode = customer.Kyc?.NaturalPersonKyc?.CustomerKbRelationship?.Code ?? string.Empty,
            Segment = customer.CustomerSegment?.SegmentKeyCode ?? string.Empty,
            ProfessionCategoryId = customer.Kyc?.NaturalPersonKyc?.Employment?.CategoryCode,
            ProfessionId = _professionTypes.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.Employment?.ProfessionCode == t.RdmCode)?.Id,
            NetMonthEarningAmountId = _netMonthEarnings.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.FinancialProfile?.NetMonthEarningCode == t.RdmCode)?.Id,
            NetMonthEarningTypeId = _incomeMainTypesAML.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.FinancialProfile?.MainSourceOfEarnings?.Code.ToString() == t.RdmCode)?.Id,
            TaxResidence = new NaturalPersonTaxResidence
            {
                ValidFrom = customer.TaxResidence.ValidFrom
            },
            LegalCapacity = new NaturalPersonLegalCapacity
            {
                RestrictionTypeId = _legalCapacityRestrictionTypes.FirstOrDefault(t => t.RdmCode == np.LegalCapacityRestriction?.RestrictionType)?.Id,
                RestrictionUntil = np.LegalCapacityRestriction?.RestrictionUntil,
            }
        };

        // tax residence countries
        person.TaxResidence.ResidenceCountries.AddRange(customer.TaxResidence.ResidenceCountries.Select(t => new NaturalPersonResidenceCountry
        {
            CountryId = _countries.FirstOrDefault(t => t.ShortName == customer.TaxResidence?.CountryCode)?.Id,
            Tin = t.Tin,
            TinMissingReasonDescription = t.TinMissingReasonDescription
        }));

        if (np.CitizenshipCodes != null && np.CitizenshipCodes.Any())
            person.CitizenshipCountriesId.AddRange(_countries.Where(t => np.CitizenshipCodes.Contains(t.ShortName)).Select(t => t.Id));

        return person;
    }

    private Contracts.IdentificationDocument? CreateIdentificationDocument(Clients.CustomerManagement.V1.IdentificationDocument? document)
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
            IdentificationDocumentTypeId = _docTypes.First(t => t.RdmCode == document.TypeCode).Id
        };
    }

    private void AddAddress(AddressTypes addressType,
                            Action<GrpcAddress> onAddAddress,
                            Address? address,
                            ComponentAddress? componentAddress,
                            DateTime? primaryAddressFrom)
    {
        if (address is null)
            return;

        onAddAddress(new GrpcAddress
        {
            AddressTypeId = (int)addressType,
            StreetNumber = componentAddress?.StreetNumber ?? string.Empty,
            HouseNumber = componentAddress?.HouseNumber ?? string.Empty,
            EvidenceNumber = componentAddress?.EvidenceNumber ?? string.Empty,
            City = address.City ?? string.Empty,
            IsPrimary = addressType == AddressTypes.Permanent,
            CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id,
            Postcode = address.PostCode?.Replace(" ", "") ?? string.Empty,
            Street = (componentAddress?.Street ?? address.Street) ?? string.Empty,
            DeliveryDetails = address.DeliveryDetails ?? string.Empty,
            CityDistrict = componentAddress?.CityDistrict ?? string.Empty,
            PragueDistrict = componentAddress?.PragueDistrict ?? string.Empty,
            CountrySubdivision = componentAddress?.CountrySubdivision ?? string.Empty,
            PrimaryAddressFrom = primaryAddressFrom,
            AddressPointId = componentAddress?.AddressPointId ?? string.Empty
        });
    }

    private static void AddContacts(CustomerBaseInfo customer, Action<Contact> onAddContact)
    {
        if (customer.PrimaryPhone is not null)
        {
            onAddContact(new Contact
            {
                ContactTypeId = (int)ContactTypes.Mobil,
                Value = customer.PrimaryPhone.PhoneNumber,
                IsPrimary = true,
                Confirmed = customer.PrimaryPhone.Confirmed
            });
        }

        if (customer.PrimaryEmail is not null)
        {
            onAddContact(new Contact
            {
                ContactTypeId = (int)ContactTypes.Email,
                Value = customer.PrimaryEmail.EmailAddress,
                IsPrimary = true,
                Confirmed = customer.PrimaryEmail.Confirmed
            });
        }
    }
}