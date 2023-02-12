using DomainServices.CustomerService.Clients;
using Newtonsoft.Json.Linq;
using NOBY.Api.Endpoints.Customer.Shared;
using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class CustomerWithChangedDataService
{
    public async Task<TResponse> GetCustomerFromCM<TResponse>(DomainServices.HouseholdService.Contracts.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
        where TResponse : Shared.BaseCustomerDetail
    {
        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // instance customer z KB CM
        var customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // convert DS contract to FE model
        return fillResponseDto<TResponse>(customer);
    }

    public async Task<TResponse> GetCustomerWithChangedData<TResponse>(DomainServices.HouseholdService.Contracts.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
        where TResponse : Shared.BaseCustomerDetail
    {
        // convert DS contract to FE model
        var model = await GetCustomerFromCM<TResponse>(customerOnSA, cancellationToken);

        // changed data already exist in database
        if (!string.IsNullOrEmpty(customerOnSA.CustomerChangeData))
        {
            // provide saved changes to original model
            var original = JObject.FromObject(model);
            var delta = JObject.Parse(customerOnSA.CustomerChangeData);

            original.Merge(delta, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace,
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            var changedData = original.ToObject<TResponse>()!;

            // https://jira.kb.cz/browse/HFICH-4200
            // docasne reseni nez se CM rozmysli jak na to
            changedData.LegalCapacity = overwriteLegalCapacity(customerOnSA);

            return changedData;
        }
        else
        {
            model.LegalCapacity = overwriteLegalCapacity(customerOnSA);
            return model;
        }
    }

    // https://jira.kb.cz/browse/HFICH-4200
    // docasne reseni nez se CM rozmysli jak na to
    private static LegalCapacityItem overwriteLegalCapacity(DomainServices.HouseholdService.Contracts.CustomerOnSA customerOnSA)
    {
        return new LegalCapacityItem()
        {
            RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
        };
    }

    private static TCustomerDetail fillResponseDto<TCustomerDetail>(DomainServices.CustomerService.Contracts.CustomerDetailResponse dsCustomer)
        where TCustomerDetail : BaseCustomerDetail
    {
        var newCustomer = (TCustomerDetail)Activator.CreateInstance(typeof(TCustomerDetail))!;
    
        NaturalPerson person = new();
        dsCustomer.NaturalPerson?.FillResponseDto(person);
        person.EducationLevelId = dsCustomer.NaturalPerson?.EducationLevelId;
        //person.ProfessionCategoryId = customer.NaturalPerson?
        //person.ProfessionId = customer.NaturalPerson ?;
        //person.NetMonthEarningAmountId = customer.NaturalPerson
        //person.NetMonthEarningTypeId = customer.NaturalPerson ?;

        newCustomer.IsBrSubscribed = dsCustomer.NaturalPerson?.IsBrSubscribed;
        
        // fill defaults
        // https://jira.kb.cz/browse/HFICH-4551
        newCustomer.HasRelationshipWithCorporate = false;
        newCustomer.HasRelationshipWithKB = false;
        newCustomer.HasRelationshipWithKBEmployee = false;
        newCustomer.IsUSPerson = false;
        newCustomer.IsAddressWhispererUsed = false;
        newCustomer.IsPoliticallyExposed = false;

        newCustomer.NaturalPerson = person;
        newCustomer.JuridicalPerson = null;
        newCustomer.IdentificationDocument = dsCustomer.IdentificationDocument?.ToResponseDto();
        newCustomer.Contacts = dsCustomer.Contacts?.ToResponseDto();
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList();

        newCustomer.CustomerIdentification = new CustomerIdentificationMethod
        {
            CzechIdentificationNumber = dsCustomer.CustomerIdentification?.CzechIdentificationNumber,
            IdentificationDate = dsCustomer.CustomerIdentification?.IdentificationDate,
            IdentificationMethodId = dsCustomer.CustomerIdentification?.IdentificationMethodId
        };

        return newCustomer;
    }

    private readonly ICustomerServiceClient _customerService;

    public CustomerWithChangedDataService(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}
