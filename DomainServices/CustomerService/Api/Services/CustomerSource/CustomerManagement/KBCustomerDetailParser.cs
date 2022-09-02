using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using DomainServices.CustomerService.Contracts;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;

public class KBCustomerDetailParser
{
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.MaritalStatuses.MaritalStatusItem> _maritals = null!;
    private List<CodebookService.Contracts.GenericCodebookItem> _titles = null!;
    private List<Endpoints.EducationLevels.EducationLevelItem> _educations = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    private KBCustomerDetailParser()
    {
    }

    public static async Task<KBCustomerDetailParser> CreateInstance(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        var instance = new KBCustomerDetailParser();

        await instance.Initialize(codebook, cancellationToken);

        return instance;
    }

    public CustomerDetailResponse Parse(CustomerBaseInfo customer)
    {
        var response = new CustomerDetailResponse
        {
            Identity = new Identity(customer.CustomerId, IdentitySchemes.Kb),
            NaturalPerson = CreateNaturalPerson(customer),
            IdentificationDocument = CreateIdentificationDocument(customer.PrimaryIdentificationDocument)
        };

        AddAddress(customer.PrimaryAddress?.Address, customer.PrimaryAddress?.ComponentAddress, AddressTypes.PERMANENT, response.Addresses.Add);
        AddAddress(customer.ContactAddress?.Address, customer.ContactAddress?.ComponentAddress, AddressTypes.MAILING, response.Addresses.Add);

        AddContacts(customer, response.Contacts.Add);

        return response;
    }

    private Task Initialize(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        return Task.WhenAll(Countries(), Genders(), Maritals(), Titles(), Educations(), DocTypes());

        async Task Countries() => _countries = await codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await codebook.Genders(cancellationToken);
        async Task Maritals() => _maritals = await codebook.MaritalStatuses(cancellationToken);
        async Task Titles() => _titles = await codebook.AcademicDegreesBefore(cancellationToken);
        async Task Educations() => _educations = await codebook.EducationLevels(cancellationToken);
        async Task DocTypes() => _docTypes = await codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private Contracts.NaturalPerson CreateNaturalPerson(CustomerBaseInfo customer)
    {
        var np = (Clients.CustomerManagement.V1.NaturalPerson)customer.Party;

        var person = new Contracts.NaturalPerson
        {
            BirthNumber = np.CzechBirthNumber ?? "",
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? "",
            LastName = np.Surname ?? "",
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id,
            BirthName = np.BirthName ?? "",
            PlaceOfBirth = np.BirthPlace ?? "",
            BirthCountryId = _countries.FirstOrDefault(t => t.ShortName == np.BirthCountryCode)?.Id,
            MaritalStatusStateId = _maritals.FirstOrDefault(t => t.RdmMaritalStatusCode == np.MaritalStatusCode)?.Id ?? 0,
            DegreeBeforeId = _titles.FirstOrDefault(t => string.Equals(t.Name, np.Title, StringComparison.InvariantCultureIgnoreCase))?.Id,
            EducationLevelId = _educations.FirstOrDefault(t => t.RdmCode.Equals(customer.Kyc?.NaturalPersonKyc?.EducationCode ?? "", StringComparison.InvariantCultureIgnoreCase))?.Id ?? 0,
            IsPoliticallyExposed = customer.IsPoliticallyExposed
        };

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
            RegisterPlace = document.RegisterPlace ?? "",
            ValidTo = document.ValidTo,
            IssuedOn = document.IssuedOn,
            IssuedBy = document.IssuedBy ?? "",
            Number = document.DocumentNumber ?? "",
            IssuingCountryId = _countries.FirstOrDefault(t => t.ShortName == document.IssuingCountryCode)?.Id,
            IdentificationDocumentTypeId = _docTypes.First(t => t.RdmCode == document.TypeCode).Id
        };
    }

    private void AddAddress(Clients.CustomerManagement.V1.Address? address,
                            ComponentAddress? componentAddress,
                            AddressTypes addressType,
                            Action<GrpcAddress> onAddAddress)
    {
        if (address is null)
            return;

        onAddAddress(new GrpcAddress
        {
            AddressTypeId = (int)addressType,
            BuildingIdentificationNumber = componentAddress?.HouseNumber ?? "",
            LandRegistryNumber = componentAddress?.StreetNumber ?? "",
            EvidenceNumber = componentAddress?.EvidenceNumber ?? "",
            City = address.City ?? "",
            IsPrimary = true,
            CountryId = _countries.FirstOrDefault(t => t.ShortName == address.CountryCode)?.Id,
            Postcode = address.PostCode ?? "",
            Street = (componentAddress?.Street ?? address.Street) ?? "",
            DeliveryDetails = address.DeliveryDetails ?? ""
        });
    }

    private void AddContacts(CustomerBaseInfo customer, Action<Contact> onAddContact)
    {
        if (customer.PrimaryPhone is not null)
        {
            onAddContact(new Contact
            {
                ContactTypeId = (int)ContactTypes.MobilPrivate,
                Value = customer.PrimaryPhone.PhoneNumber, 
                IsPrimary = true
            });
        }

        if (customer.PrimaryEmail is not null)
        {
            onAddContact(new Contact
            {
                ContactTypeId = (int)ContactTypes.Email,
                Value = customer.PrimaryEmail.EmailAddress,
                IsPrimary = true
            });
        }
    }
}