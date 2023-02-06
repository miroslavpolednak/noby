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

        // https://jira.kb.cz/browse/HFICH-4200
        if (customerOnSA.CustomerAdditionalData?.LegalCapacity is not null)
        {
            model.LegalCapacity = new()
            {
                RestrictionTypeId = customerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionTypeId,
                RestrictionUntil = customerOnSA.CustomerAdditionalData.LegalCapacity.RestrictionUntil
            };
        }

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
            changedData.LegalCapacity = new()
            {
                RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
                RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
            };

            return changedData;
        }
        else
        {
            return model;
        }
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
        //newCustomer.HasRelationshipWithCorporate = customer.NaturalPerson?.HasRelationshipWithCorporate,
        //newCustomer.HasRelationshipWithKB = customer.NaturalPerson?.HasRelationshipWithKB,
        //newCustomer.HasRelationshipWithKBEmployee = customer.NaturalPerson?.HasRelationshipWithKBEmployee,
        //newCustomer.IsUSPerson = customer.NaturalPerson?.IsUSPerson,
        //newCustomer.IsAddressWhispererUsed = customer.NaturalPerson?.AddressWhispererUsed,
        //newCustomer.IsPoliticallyExposed = customer.NaturalPerson?.IsPoliticallyExposed,
        newCustomer.NaturalPerson = person;
        newCustomer.JuridicalPerson = null;
        newCustomer.IdentificationDocument = dsCustomer.IdentificationDocument?.ToResponseDto();
        newCustomer.Contacts = dsCustomer.Contacts?.ToResponseDto();
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList();

        return newCustomer;
    }

    private readonly ICustomerServiceClient _customerService;

    public CustomerWithChangedDataService(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}
