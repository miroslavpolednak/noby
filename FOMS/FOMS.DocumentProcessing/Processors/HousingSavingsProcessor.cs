using System.Linq;

namespace FOMS.DocumentProcessing;

internal class HousingSavingsProcessor : BaseDocumentProcessor, IDocumentProcessor
{
    public HousingSavingsProcessor(
        ServiceAccessor serviceAccessor, 
        DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse salesArrangement)
        : base(serviceAccessor, salesArrangement) 
    { }

    private DocumentContracts.HousingSavings.HousingSavingsContract getNewContract()
    {
        var model = new DocumentContracts.HousingSavings.HousingSavingsContract();

        return model;
    }

    public async Task<object> GetPart(int partId)
    {
        var customerService = _serviceAccessor.GetRequiredService<DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction>();
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? getNewContract();

        var customer = CIS.Core.Results.ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.GetDetailResponse>(await customerService.GetDetail(new DomainServices.CustomerService.Contracts.GetDetailRequest
        {
            Identity = saObject.Customer?.Id ?? 0
        }));

        var address = customer.Addresses.First(t => t.Type == DomainServices.CustomerService.Contracts.AddressTypes.Pernament);
        saObject.Customer = new DocumentContracts.SharedModels.CustomerDetail
        {
            Id = saObject.Customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            BirthNumber = customer.BirthNumber,
            DateOfBirth = customer.DateOfBirth,
            DegreeBefore = customer.DegreeBefore,
            DegreeAfter = customer.DegreeAfter,
            PlaceOfBirth = customer.PlaceOfBirth,
            HomeAddress = new DocumentContracts.SharedModels.Address
            {
                Street = address.Street,
                BuildingIdentificationNumber = address.BuildingIdentificationNumber,
                LandRegistryNumber = address.LandRegistryNumber,
                City = address.City,
                Postcode = address.Postcode
            },
            Contacts = new DocumentContracts.SharedModels.Contacts
            {
                Email = customer.Contacts.FirstOrDefault(t => t.Type == DomainServices.CustomerService.Contracts.Contact.Types.ContactTypes.Email)?.Value,
                Mobile = customer.Contacts.FirstOrDefault(t => t.Type == DomainServices.CustomerService.Contracts.Contact.Types.ContactTypes.MobilePrivate)?.Value
            }
        };
        
        return saObject.GetPart(partId);
    }

    public async Task SavePart(int partId, object data)
    {
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? new DocumentContracts.HousingSavings.HousingSavingsContract();

        saObject.MergePart(partId, data);

        saveSalesArrangementData(saObject);
    }
}
