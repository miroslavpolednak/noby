using System.Globalization;
using CIS.Foms.Enums;
using DomainServices.CodebookService.Clients;
using CM = DomainServices.CustomerService.ExternalServices.CustomerManagement.V2;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CustomerService.Api.Services.CustomerManagement;

[ScopedService, SelfService]
internal sealed class CustomerManagementDetailProvider
{
    private readonly CM.ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceClient _codebook;

    private List<CountriesResponse.Types.CountryItem> _countries = null!;
    private List<GendersResponse.Types.GenderItem> _genders = null!;
    private List<MaritalStatusesResponse.Types.MaritalStatuseItem> _maritals = null!;
    private List<GenericCodebookResponse.Types.GenericCodebookItem> _titles = null!;
    private List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem> _professionTypes = null!;
    private List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem> _netMonthEarnings = null!;
    private List<LegalCapacityRestrictionTypesResponse.Types.LegalCapacityRestrictionTypeItem> _legalCapacityRestrictionTypes = null!;
    private List<GenericCodebookWithRdmCodeResponse.Types.GenericCodebookWithRdmCodeItem> _incomeMainTypesAML = null!;
    private List<EducationLevelsResponse.Types.EducationLevelItem> _educations = null!;
    private List<IdentificationDocumentTypesResponse.Types.IdentificationDocumentTypeItem> _docTypes = null!;

    public CustomerManagementDetailProvider(CM.ICustomerManagementClient customerManagement, ICodebookServiceClient codebook)
    {
        _customerManagement = customerManagement;
        _codebook = codebook;
    }

    public async Task<CustomerDetailResponse> GetDetail(long customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerManagement.GetDetail(customerId, cancellationToken);

        await InitializeCodebooks(cancellationToken);

        return CreateDetailResponse(customer);
    }

    public async Task<IEnumerable<CustomerDetailResponse>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        var customers = await _customerManagement.GetList(customerIds, cancellationToken);

        if (!customers.Any())
            return Enumerable.Empty<CustomerDetailResponse>();

        await InitializeCodebooks(cancellationToken);

        return customers.Select(CreateDetailResponse);
    }

    private CustomerDetailResponse CreateDetailResponse(CM.Contracts.CustomerInfo customer)
    {
        var response = new CustomerDetailResponse
        {
            Identities = { new Identity(customer.CustomerId, IdentitySchemes.Kb) },
            NaturalPerson = CreateNaturalPerson(customer, customer.IsLegallyIncapable),
            IdentificationDocument = CreateIdentificationDocument(customer.PrimaryIdentificationDocument),
            CustomerIdentification = CreateCustomerIdentification(customer.CustomerIdentification)
        };

        AddAddress(AddressTypes.Permanent, response.Addresses.Add, customer.PrimaryAddress?.ComponentAddressPoint, customer.PrimaryAddress?.SingleLineAddressPoint, null);
        AddAddress(AddressTypes.Mailing, response.Addresses.Add, customer.ContactAddress?.ComponentAddressPoint, customer.ContactAddress?.SingleLineAddressPoint, customer.ContactAddress?.Confirmed);
        AddAddress(AddressTypes.Other, response.Addresses.Add, customer.TemporaryStay?.ComponentAddressPoint, customer.TemporaryStay?.SingleLineAddressPoint, null);

        AddContacts(customer, response.Contacts.Add);

        return response;
    }

    private Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        return Task.WhenAll(Countries(), Genders(), Maritals(), Titles(), Educations(), DocTypes(), ProfessionTypes(), NetMonthEarnings(), IncomeMainTypesAML(), LegalCapacityRestrictionTypes());

        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await _codebook.Genders(cancellationToken);
        async Task Maritals() => _maritals = await _codebook.MaritalStatuses(cancellationToken);
        async Task Titles() => _titles = await _codebook.AcademicDegreesBefore(cancellationToken);
        async Task Educations() => _educations = await _codebook.EducationLevels(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
        async Task ProfessionTypes() => _professionTypes = await _codebook.ProfessionTypes(cancellationToken);
        async Task NetMonthEarnings() => _netMonthEarnings = await _codebook.NetMonthEarnings(cancellationToken);
        async Task IncomeMainTypesAML() => _incomeMainTypesAML = await _codebook.IncomeMainTypesAML(cancellationToken);
        async Task LegalCapacityRestrictionTypes() => _legalCapacityRestrictionTypes = await _codebook.LegalCapacityRestrictionTypes(cancellationToken);
    }

    private NaturalPerson CreateNaturalPerson(CM.Contracts.CustomerInfo customer, bool? isLegallyIncapable)
    {
        var np = customer.Party.NaturalPersonAttributes;

        var person = new NaturalPerson
        {
            BirthNumber = np.CzechBirthNumber ?? string.Empty,
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? string.Empty,
            LastName = np.Surname ?? string.Empty,
            GenderId = _genders.FirstOrDefault(t => t.KbCmCode == np.GenderCode)?.Id ?? 0,
            BirthName = np.BirthName ?? string.Empty,
            PlaceOfBirth = np.BirthPlace ?? string.Empty,
            BirthCountryId = _countries.FirstOrDefault(t => t.ShortName == np.BirthCountryCode)?.Id,
            MaritalStatusStateId = _maritals.FirstOrDefault(t => t.RdmMaritalStatusCode == np.MaritalStatusCode)?.Id ?? 0,
            DegreeBeforeId = _titles.FirstOrDefault(t => string.Equals(t.Name, np.Title, StringComparison.OrdinalIgnoreCase))?.Id,
            EducationLevelId = _educations.FirstOrDefault(t => t.RdmCode.Equals(customer.Kyc?.NaturalPersonKyc?.EducationCode ?? "", StringComparison.OrdinalIgnoreCase))?.Id ?? 0,
            IsPoliticallyExposed = customer.IsPoliticallyExposed,
            IsUSPerson = false, //je vzdy false!
            IsBrSubscribed = customer.BrSubscription?.IsSubscribed ?? false,
            KbRelationshipCode = customer.Kyc?.NaturalPersonKyc?.CustomerKbRelationship?.Code ?? string.Empty,
            Segment = customer.CustomerSegment?.SegmentKeyCode ?? string.Empty,
            ProfessionCategoryId = customer.Kyc?.NaturalPersonKyc?.Employment?.CategoryCode,
            ProfessionId = _professionTypes.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.Employment?.ProfessionCode.ToString() == t.RdmCode)?.Id,
            NetMonthEarningAmountId = _netMonthEarnings.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.FinancialProfile?.NetMonthEarningCode == t.RdmCode)?.Id,
            NetMonthEarningTypeId = _incomeMainTypesAML.FirstOrDefault(t => customer.Kyc?.NaturalPersonKyc?.FinancialProfile?.MainSourceOfEarnings?.Code.ToString(System.Globalization.CultureInfo.InvariantCulture) == t.RdmCode)?.Id,
            TaxResidence = new NaturalPersonTaxResidence
            {
                ValidFrom = customer.TaxResidence?.ValidFrom
            },
            LegalCapacity = new NaturalPersonLegalCapacity
            {
                RestrictionTypeId = isLegallyIncapable switch  //MOCK HFICH-5860
                {
                    true => 2,
                    false => 1,
                    null => 3
                },
                RestrictionUntil = DateTime.Now.AddYears(50) //Mock HFICH-5860
                //RestrictionTypeId = _legalCapacityRestrictionTypes.FirstOrDefault(t => t.RdmCode == np.LegalCapacityRestriction?.RestrictionType)?.Id,
                //RestrictionUntil = np.LegalCapacityRestriction?.RestrictionUntil
            }
        };

        // tax residence countries
        if (customer.TaxResidence?.ResidenceCountries?.Any() ?? false)
        {
            person.TaxResidence.ResidenceCountries.AddRange(customer.TaxResidence.ResidenceCountries.Select(t => new NaturalPersonResidenceCountry
            {
                CountryId = _countries.FirstOrDefault(c => c.ShortName == t.CountryCode)?.Id,
                Tin = t.Tin,
                TinMissingReasonDescription = t.TinMissingReasonDescription
            }));
        }

        if (np.CitizenshipCodes != null && np.CitizenshipCodes.Any())
            person.CitizenshipCountriesId.AddRange(_countries.Where(t => np.CitizenshipCodes.Contains(t.ShortName)).Select(t => t.Id));

        return person;
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
            IdentificationDocumentTypeId = _docTypes.First(t => t.RdmCode == document.TypeCode).Id
        };
    }

    private static CustomerIdentification? CreateCustomerIdentification(CM.Contracts.CustomerIdentification? customerIdentification)
    {
        if (customerIdentification is null)
            return default;

        return new CustomerIdentification
        {
            IdentificationMethodId = int.Parse(customerIdentification.IdentificationMethodCode, CultureInfo.InvariantCulture),
            IdentificationDate = customerIdentification.IdentificationDate,
            CzechIdentificationNumber = customerIdentification.CzechIdentificationNumber
        };
    }

    private void AddAddress(AddressTypes addressType, Action<GrpcAddress> onAddAddress, CM.Contracts.ComponentAddressPoint? componentAddress, CM.Contracts.SingleLineAddressPoint? singleLineAddress, bool? isConfirmed)
    {
        if (componentAddress is null)
            return;

        onAddAddress(new GrpcAddress
        {
            AddressTypeId = (int)addressType,
            StreetNumber = componentAddress.StreetNumber ?? string.Empty,
            HouseNumber = componentAddress.HouseNumber ?? string.Empty,
            EvidenceNumber = componentAddress.EvidenceNumber ?? string.Empty,
            City = componentAddress.City ?? string.Empty,
            IsPrimary = addressType == AddressTypes.Permanent,
            CountryId = _countries.FirstOrDefault(t => t.ShortName == componentAddress.CountryCode)?.Id,
            Postcode = componentAddress.PostCode?.Replace(" ", "") ?? string.Empty,
            Street = componentAddress.Street ?? string.Empty,
            DeliveryDetails = componentAddress.DeliveryDetails ?? string.Empty,
            CityDistrict = componentAddress.CityDistrict ?? string.Empty,
            PragueDistrict = componentAddress.PragueDistrict ?? string.Empty,
            CountrySubdivision = componentAddress.CountrySubdivision ?? string.Empty,
            AddressPointId = componentAddress.AddressPointId ?? string.Empty,
            SingleLineAddressPoint = singleLineAddress?.Address,
            IsAddressConfirmed = isConfirmed
        });
    }

    private static void AddContacts(CM.Contracts.CustomerInfo customer, Action<Contact> onAddContact)
    {
        if (customer.PrimaryPhone is not null)
        {
            var phone = new Contact
            {
                ContactTypeId = (int)ContactTypes.Mobil,
                IsPrimary = true,
                Mobile = new MobilePhoneItem
                {
                    PhoneIDC = customer.PrimaryPhone.ComponentPhone.PhoneIDC,
                    PhoneNumber = customer.PrimaryPhone.ComponentPhone.PhoneNumber,
                    IsPhoneConfirmed = customer.PrimaryPhone.Confirmed
                }
            };

            onAddContact(phone);
        }

        if (customer.PrimaryEmail is not null)
        {
            var email = new Contact
            {
                ContactTypeId = (int)ContactTypes.Email,
                IsPrimary = true,
                Email = new EmailAddressItem
                {
                    EmailAddress = customer.PrimaryEmail.EmailAddress,
                    IsEmailConfirmed = customer.PrimaryEmail.Confirmed
                },
            };

            onAddContact(email);
        }
    }
}