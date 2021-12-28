using CIS.Core.Results;
using System.Linq;

namespace FOMS.DocumentProcessing;

internal class HousingSavingsProcessor : BaseDocumentProcessor, IDocumentProcessor
{
    public HousingSavingsProcessor(
        ServiceAccessor serviceAccessor, 
        DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse salesArrangement)
        : base(serviceAccessor, salesArrangement) 
    { }

    private async Task<DocumentContracts.HousingSavings.HousingSavingsContract> getNewContract()
    {
        // data ze simulace
        var offerService = _serviceAccessor.GetRequiredService<DomainServices.OfferService.Abstraction.IOfferServiceAbstraction>();
        var offerInstance = ServiceCallResult.Resolve<DomainServices.OfferService.Contracts.GetBuildingSavingsDataResponse>(await offerService.GetBuildingSavingsData(_salesArrangement.OfferInstanceId.GetValueOrDefault()));

        // info o dealerovi
        var dealer = await getCurrentUserInfo();

        // info o customerovi
        var customerService = _serviceAccessor.GetRequiredService<DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction>();
        var customer = ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.GetDetailResponse>(await customerService.GetDetail(new()
        {
            Identity = _salesArrangement.SalesArrangementId
        }));
        var address = customer.Addresses.FirstOrDefault(t => t.Type == DomainServices.CustomerService.Contracts.AddressTypes.Pernament);

        DocumentContracts.HousingSavings.HousingSavingsContract model = new()
        {
            Customer = new()
            {
                Id = _salesArrangement.SalesArrangementId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                BirthNumber = customer.BirthNumber,
                DateOfBirth = customer.DateOfBirth,
                DegreeBefore = customer.DegreeBefore,
                DegreeAfter = customer.DegreeAfter,
                PlaceOfBirth = customer.PlaceOfBirth,
                HomeAddress = address is null ? null : new DocumentContracts.SharedModels.Address
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
            },
            Citizenship = new()
            {
                HasCzechCitizenship = true
            },
            Finaces = new()
            {
                TargetAmount = offerInstance.InputData.TargetAmount,
                ActionCode = offerInstance.InputData.ActionCode
            },
            Dealer = dealer
        };

        return model;
    }

    public async Task<object> GetPart(int partId)
    {
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? await getNewContract();
   
        return saObject.GetPart(partId);
    }

    public async Task SavePart(int partId, object data)
    {
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? new DocumentContracts.HousingSavings.HousingSavingsContract();

        saObject.MergePart(partId, data);

        saveSalesArrangementData(saObject);
    }
}
