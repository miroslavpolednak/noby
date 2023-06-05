using DomainServices.CustomerService.Clients;
using Newtonsoft.Json.Linq;
using NOBY.Api.Endpoints.Customer.Shared;
using NOBY.Dto;
using __Household = DomainServices.HouseholdService.Contracts;
using __Customer = DomainServices.CustomerService.Contracts;
using CIS.Foms.Enums;
using NOBY.Api.Extensions;

namespace NOBY.Api.Endpoints.Customer;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
internal sealed class CustomerWithChangedDataService
{
    public async Task<(TResponse Customer, int? IdentificationMethodId)> GetCustomerFromCM<TResponse>(__Household.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
        where TResponse : Shared.BaseCustomerDetail
    {
        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // instance customer z KB CM
        var customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // convert DS contract to FE model
        return (fillResponseDto<TResponse>(customer, customerOnSA), customer.CustomerIdentification?.IdentificationMethodId);
    }

    public async Task<(TResponse Customer, int? IdentificationMethodId)> GetCustomerWithChangedData<TResponse>(__Household.CustomerOnSA customerOnSA, CancellationToken cancellationToken)
        where TResponse : Shared.BaseCustomerDetail
    {
        // convert DS contract to FE model
        var model = await GetCustomerFromCM<TResponse>(customerOnSA, cancellationToken);

        // changed data already exist in database
        if (!string.IsNullOrEmpty(customerOnSA.CustomerChangeData))
        {
            // provide saved changes to original model
            var original = JObject.FromObject(model.Customer);
            var delta = JObject.Parse(customerOnSA.CustomerChangeData);

            original.Merge(delta, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace,
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            return (original.ToObject<TResponse>()!, model.IdentificationMethodId);
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
        person.TaxResidences = new TaxResidenceItem
        {
            validFrom = dsCustomer.NaturalPerson?.TaxResidence?.ValidFrom,
            ResidenceCountries = dsCustomer.NaturalPerson?.TaxResidence?.ResidenceCountries.Select(c => new TaxResidenceCountryItem
            {
                CountryId = c.CountryId,
                Tin = c.Tin
            }).ToList()
        };
        person.ProfessionCategoryId = dsCustomer.NaturalPerson?.ProfessionCategoryId;
        person.ProfessionId = dsCustomer.NaturalPerson?.ProfessionId;
        person.NetMonthEarningAmountId = dsCustomer.NaturalPerson?.NetMonthEarningAmountId;
        person.NetMonthEarningTypeId = dsCustomer.NaturalPerson?.NetMonthEarningTypeId;
        newCustomer.IsBrSubscribed = dsCustomer.NaturalPerson?.IsBrSubscribed;
        
        newCustomer.HasRelationshipWithCorporate = customerOnSA.CustomerAdditionalData?.HasRelationshipWithCorporate;
        newCustomer.HasRelationshipWithKB = customerOnSA.CustomerAdditionalData?.HasRelationshipWithKB;
        newCustomer.HasRelationshipWithKBEmployee = customerOnSA.CustomerAdditionalData?.HasRelationshipWithKBEmployee;
        newCustomer.IsUSPerson = customerOnSA.CustomerAdditionalData?.IsUSPerson;
        newCustomer.IsPoliticallyExposed = customerOnSA.CustomerAdditionalData?.IsPoliticallyExposed;

        newCustomer.NaturalPerson = person;
        newCustomer.JuridicalPerson = null;
        newCustomer.IdentificationDocument = dsCustomer.IdentificationDocument?.ToResponseDto();
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList();

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
            contactsDetail.EmailAddress = getEmail<EmailAddressConfirmedDto>(dsCustomer);
            contactsDetail.MobilePhone = getPhone<PhoneNumberConfirmedDto>(dsCustomer);
        }
        else if (newCustomer is ICustomerDetailContacts)
        {
            var contactsDetail = (ICustomerDetailContacts)newCustomer;
            contactsDetail.EmailAddress = getEmail<EmailAddressDto>(dsCustomer);
            contactsDetail.MobilePhone = getPhone<PhoneNumberDto>(dsCustomer);
        }

        return newCustomer;
    }

    private static TPhone? getPhone<TPhone>(__Customer.CustomerDetailResponse dsCustomer)
        where TPhone : IPhoneNumberDto
    {
        var phone = dsCustomer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
        if (!string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber))
        {
            var newPhone = (TPhone)Activator.CreateInstance(typeof(TPhone))!;
            newPhone.IsConfirmed = phone.Mobile.IsPhoneConfirmed;
            newPhone.PhoneNumber = phone.Mobile.PhoneNumber;
            newPhone.PhoneIDC = phone.Mobile.PhoneIDC;
            return newPhone;
        }
        else
            return default(TPhone);
    }

    private static TEmail? getEmail<TEmail>(__Customer.CustomerDetailResponse dsCustomer)
        where TEmail : IEmailAddressDto
    {
        var email = dsCustomer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);
        if (!string.IsNullOrEmpty(email?.Email?.EmailAddress))
        {
            var newEmail = (TEmail)Activator.CreateInstance(typeof(TEmail))!;
            newEmail.IsConfirmed = email.Email.IsEmailConfirmed;
            newEmail.EmailAddress = email.Email.EmailAddress;
            return newEmail;
        }
        else
            return default(TEmail);
    }

    private readonly ICustomerServiceClient _customerService;

    public CustomerWithChangedDataService(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}
