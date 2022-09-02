using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using DomainServices.CustomerService.Contracts;
using DomainServices.CodebookService.Abstraction;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;

public class KBCustomerListItemParser
{
    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    private KBCustomerListItemParser()
    {
    }

    public static async Task<KBCustomerListItemParser> CreateInstance(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        var instance = new KBCustomerListItemParser();

        await instance.Initialize(codebook, cancellationToken);

        return instance;
    }

    public CustomerListItem Parse(CustomerBaseInfo customer)
    {
        return new CustomerListItem
        {
            Identity = new Identity(customer.CustomerId, IdentitySchemes.Kb),
            NaturalPerson = CreateNaturalPerson(customer),
            IdentificationDocument = CreateIdentificationDocument(customer.PrimaryIdentificationDocument)
        };
    }

    private async Task Initialize(ICodebookServiceAbstraction codebook, CancellationToken cancellationToken)
    {
        await Task.WhenAll(Countries(), Genders(), DocTypes());

        async Task Countries() => _countries = await codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await codebook.Genders(cancellationToken);
        async Task DocTypes() => _docTypes = await codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private NaturalPersonBasicInfo CreateNaturalPerson(CustomerBaseInfo customer)
    {
        var np = (Clients.CustomerManagement.V1.NaturalPerson)customer.Party;

        return new NaturalPersonBasicInfo
        {
            BirthNumber = np.CzechBirthNumber ?? "",
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? "",
            LastName = np.Surname ?? "",
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id
        };
    }

    private Contracts.IdentificationDocument? CreateIdentificationDocument(Clients.CustomerManagement.V1.IdentificationDocument document)
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
}