using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;
using System.Diagnostics;
using CIS.Foms.Enums;
using Endpoints = DomainServices.CodebookService.Contracts.Endpoints;

namespace DomainServices.CustomerService.Api.Services.CustomerSource.CustomerManagement;

[ScopedService, SelfService]
internal class CustomerManagementListProvider
{
    private readonly ICustomerManagementClient _customerManagement;
    private readonly ICodebookServiceAbstraction _codebook;

    private List<Endpoints.Countries.CountriesItem> _countries = null!;
    private List<Endpoints.Genders.GenderItem> _genders = null!;
    private List<Endpoints.IdentificationDocumentTypes.IdentificationDocumentTypesItem> _docTypes = null!;

    public CustomerManagementListProvider(ICustomerManagementClient customerManagement, ICodebookServiceAbstraction codebook)
    {
        _customerManagement = customerManagement;
        _codebook = codebook;
    }

    public async Task<IEnumerable<CustomerListItem>> GetList(IEnumerable<long> customerIds, CancellationToken cancellationToken)
    {
        var customers = await _customerManagement.GetList(customerIds, Activity.Current?.TraceId.ToHexString() ?? "", cancellationToken);

        await InitializeCodebooks(cancellationToken);

        return customers.Select(c => new CustomerListItem
        {
            Identity = new Identity(c.CustomerId, IdentitySchemes.Kb),
            NaturalPerson = CreateNaturalPerson(c),
            IdentificationDocument = CreateIdentificationDocument(c.PrimaryIdentificationDocument)
        });
    }

    private async Task InitializeCodebooks(CancellationToken cancellationToken)
    {
        await Task.WhenAll(Countries(), Genders(), DocTypes());

        async Task Countries() => _countries = await _codebook.Countries(cancellationToken);
        async Task Genders() => _genders = await _codebook.Genders(cancellationToken);
        async Task DocTypes() => _docTypes = await _codebook.IdentificationDocumentTypes(cancellationToken);
    }

    private NaturalPersonBasicInfo CreateNaturalPerson(CustomerBaseInfo customer)
    {
        var np = (Clients.CustomerManagement.V1.NaturalPerson)customer.Party;

        return new NaturalPersonBasicInfo
        {
            BirthNumber = np.CzechBirthNumber ?? string.Empty,
            DateOfBirth = np.BirthDate,
            FirstName = np.FirstName ?? string.Empty,
            LastName = np.Surname ?? string.Empty,
            GenderId = _genders.First(t => t.KbCmCode == np.GenderCode.ToString()).Id
        };
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
}