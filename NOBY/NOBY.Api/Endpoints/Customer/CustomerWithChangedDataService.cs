using DomainServices.CustomerService.Clients;
using Newtonsoft.Json.Linq;
using NOBY.Api.Endpoints.Customer.Shared;
using NOBY.Api.SharedDto;
using __Household = DomainServices.HouseholdService.Contracts;
using __Customer = DomainServices.CustomerService.Contracts;
using CIS.Foms.Enums;

namespace NOBY.Api.Endpoints.Customer;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class CustomerWithChangedDataService
{
    public async Task<TResponse> GetCustomerFromCM<TResponse>(__Household.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
        where TResponse : Shared.BaseCustomerDetail
    {
        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // instance customer z KB CM
        var customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // convert DS contract to FE model
        return fillResponseDto<TResponse>(customer, customerOnSA);
    }

    public async Task<TResponse> GetCustomerWithChangedData<TResponse>(__Household.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
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

            return original.ToObject<TResponse>()!;
        }
        else
        {
            return model;
        }
    }

    private static TCustomerDetail fillResponseDto<TCustomerDetail>(__Customer.CustomerDetailResponse dsCustomer, __Household.CustomerOnSA customerOnSA)
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
        
        newCustomer.HasRelationshipWithCorporate = customerOnSA.CustomerAdditionalData?.HasRelationshipWithCorporate;
        newCustomer.HasRelationshipWithKB = customerOnSA.CustomerAdditionalData?.HasRelationshipWithKB;
        newCustomer.HasRelationshipWithKBEmployee = customerOnSA.CustomerAdditionalData?.HasRelationshipWithKBEmployee;
        newCustomer.IsUSPerson = customerOnSA.CustomerAdditionalData?.IsUSPerson;
        newCustomer.IsAddressWhispererUsed = customerOnSA.CustomerAdditionalData?.IsAddressWhispererUsed;
        newCustomer.IsPoliticallyExposed = customerOnSA.CustomerAdditionalData?.IsPoliticallyExposed;

        newCustomer.NaturalPerson = person;
        newCustomer.JuridicalPerson = null;
        newCustomer.IdentificationDocument = dsCustomer.IdentificationDocument?.ToResponseDto();
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList();

        newCustomer.CustomerIdentification = new CustomerIdentificationMethod
        {
            CzechIdentificationNumber = dsCustomer.CustomerIdentification?.CzechIdentificationNumber,
            IdentificationDate = dsCustomer.CustomerIdentification?.IdentificationDate,
            IdentificationMethodId = dsCustomer.CustomerIdentification?.IdentificationMethodId
        };

        // https://jira.kb.cz/browse/HFICH-4200
        // docasne reseni nez se CM rozmysli jak na to
        newCustomer.LegalCapacity = new LegalCapacityItem()
        {
            RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
        };

        if (newCustomer is ICustomerDetailConfirmedContacts)
        {
            var contactsDetail = (ICustomerDetailConfirmedContacts)newCustomer;

            var email = dsCustomer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);
            if (!string.IsNullOrEmpty(email?.Email?.Address))
            {
                contactsDetail.PrimaryEmail = new CustomerDetailEmailConfirmedDto
                {
                    IsConfirmed = email.IsConfirmed,
                    EmailAddress = email.Email.Address
                };
            }

            var phone = dsCustomer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
            if (!string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber))
            {
                contactsDetail.PrimaryPhoneNumber = new CustomerDetailPhoneConfirmedDto
                {
                    IsConfirmed = phone.IsConfirmed,
                    PhoneNumber = phone.Mobile.PhoneNumber,
                    PhoneIDC = phone.Mobile.PhoneNumber
                };
            }
        }

        return newCustomer;
    }

    private readonly ICustomerServiceClient _customerService;

    public CustomerWithChangedDataService(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}
