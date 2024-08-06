using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Contracts.Model;
using NOBY.ApiContracts;
using SharedTypes.GrpcTypes;
using SharedTypes.Types;

namespace NOBY.Services.Customer;

public static class CustomerMapper
{
    public static CustomerChangeData? MapCustomerDtoToChangeData<TCustomerDetail>(TCustomerDetail? customerDto) where TCustomerDetail : CustomerDetailBase
    {
        if (customerDto is null)
            return null;
        
        var changeData = new CustomerChangeData
        {
            NaturalPerson = customerDto.NaturalPerson is null ? null : new NaturalPersonDelta()
            {
                BirthNumber = customerDto.NaturalPerson.BirthNumber,
                FirstName = customerDto.NaturalPerson.FirstName,
                LastName = customerDto.NaturalPerson.LastName,
                DateOfBirth = customerDto.NaturalPerson.DateOfBirth,
                DegreeBeforeId = customerDto.NaturalPerson.DegreeBeforeId,
                DegreeAfterId = customerDto.NaturalPerson.DegreeAfterId,
                BirthName = customerDto.NaturalPerson.BirthName,
                PlaceOfBirth = customerDto.NaturalPerson.PlaceOfBirth,
                BirthCountryId = customerDto.NaturalPerson.BirthCountryId,
                Gender = (Genders)(int)customerDto.NaturalPerson.Gender,
                MaritalStatusId = customerDto.NaturalPerson.MaritalStatusId,
                CitizenshipCountriesId = customerDto.NaturalPerson.CitizenshipCountriesId,
                EducationLevelId = customerDto.NaturalPerson.EducationLevelId,
                ProfessionCategoryId = customerDto.NaturalPerson.ProfessionCategoryId,
                ProfessionId = customerDto.NaturalPerson.ProfessionId,
                NetMonthEarningAmountId = customerDto.NaturalPerson.NetMonthEarningAmountId,
                NetMonthEarningTypeId = customerDto.NaturalPerson.NetMonthEarningTypeId,
                LegalCapacity = customerDto.NaturalPerson.LegalCapacity is null ? null : new NaturalPersonDelta.LegalCapacityDelta()
                {
                    RestrictionTypeId = customerDto.NaturalPerson.LegalCapacity.RestrictionTypeId,
                    RestrictionUntil = customerDto.NaturalPerson.LegalCapacity.RestrictionUntil
                },
                TaxResidences = customerDto.NaturalPerson.TaxResidences is null ? null : new NaturalPersonDelta.TaxResidenceDelta()
                {
                    ValidFrom = customerDto.NaturalPerson.TaxResidences.ValidFrom,
                    ResidenceCountries = customerDto.NaturalPerson.TaxResidences.ResidenceCountries?.Select(c => new NaturalPersonDelta.TaxResidenceDelta.TaxResidenceItemDelta
                    {
                        CountryId = c.CountryId,
                        Tin = c.Tin,
                        TinMissingReasonDescription = c.TinMissingReasonDescription
                    }).ToList()
                }
            },
            IdentificationDocument = customerDto.IdentificationDocument is null ? null : new IdentificationDocumentDelta
            {
                IdentificationDocumentTypeId = customerDto.IdentificationDocument.IdentificationDocumentTypeId ?? 0,
                IssuingCountryId = customerDto.IdentificationDocument.IssuingCountryId,
                IssuedBy = customerDto.IdentificationDocument.IssuedBy,
                ValidTo = customerDto.IdentificationDocument.ValidTo?.ToDateTime(TimeOnly.MinValue),
                IssuedOn = customerDto.IdentificationDocument.IssuedOn?.ToDateTime(TimeOnly.MinValue),
                RegisterPlace = customerDto.IdentificationDocument.RegisterPlace,
                Number = customerDto.IdentificationDocument.Number ?? string.Empty,
            },
            Addresses = customerDto.Addresses?.Select(a => (GrpcAddress)a!).Select(a => (Address)a!).ToList(),
        };

        if (customerDto is CustomerGetCustomerDetailWithChangesResponse confirmedContactsDetail)
        {
            changeData.MobilePhone = confirmedContactsDetail.MobilePhone is null ? null : new MobilePhoneDelta
            {
                PhoneNumber = confirmedContactsDetail.MobilePhone.PhoneNumber,
                PhoneIDC = confirmedContactsDetail.MobilePhone.PhoneIDC
            };

            changeData.EmailAddress = confirmedContactsDetail.EmailAddress is null ? null : new EmailAddressDelta
            {
                EmailAddress = confirmedContactsDetail.EmailAddress.EmailAddress
            };
        }

        if (customerDto is CustomerUpdateCustomerDetailWithChangesRequest contactsDetail)
        {
            changeData.MobilePhone = contactsDetail.MobilePhone is null ? null : new MobilePhoneDelta
            {
                PhoneNumber = contactsDetail.MobilePhone.PhoneNumber,
                PhoneIDC = contactsDetail.MobilePhone.PhoneIDC
            };

            changeData.EmailAddress = contactsDetail.EmailAddress is null ? null : new EmailAddressDelta
            {
                EmailAddress = contactsDetail.EmailAddress.EmailAddress
            };
        }

        return changeData;
    }

    public static TCustomerDetail MapCustomerToResponseDto<TCustomerDetail>(CustomerDetailResponse dsCustomer, CustomerOnSA customerOnSA) where TCustomerDetail : CustomerDetailBase
    {
        var newCustomer = (TCustomerDetail)Activator.CreateInstance(typeof(TCustomerDetail))!;

        CustomerNaturalPerson person = new();
        dsCustomer.NaturalPerson?.FillResponseDto(person);
        person.EducationLevelId = dsCustomer.NaturalPerson?.EducationLevelId;
        person.TaxResidences = new CustomerTaxResidenceItem
        {
            ValidFrom = dsCustomer.NaturalPerson?.TaxResidence?.ValidFrom,
            ResidenceCountries = dsCustomer.NaturalPerson?.TaxResidence?.ResidenceCountries.Select(c => new CustomerTaxResidenceCountryItem
            {
                CountryId = c.CountryId,
                Tin = c.Tin,
                TinMissingReasonDescription = c.TinMissingReasonDescription
            }).ToList()
        };
        person.ProfessionCategoryId = dsCustomer.NaturalPerson?.ProfessionCategoryId ?? 0;
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
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (SharedTypesAddress)t!).ToList();

        // https://jira.kb.cz/browse/HFICH-4200
        // docasne reseni nez se CM rozmysli jak na to
        newCustomer.LegalCapacity = new CustomerLegalCapacityItem
        {
            RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
        };

        var email = GetEmail(dsCustomer);
        var phone = GetPhone(dsCustomer);

        if (newCustomer is CustomerGetCustomerDetailWithChangesResponse confirmedContactsDetail)
        {
            confirmedContactsDetail.EmailAddress = email is null ? null : new SharedTypesEmailConfirmed
            {
                EmailAddress = email.Email.EmailAddress,
                IsConfirmed = email.Email.IsEmailConfirmed
            };

            confirmedContactsDetail.MobilePhone = phone is null ? null : new SharedTypesPhoneConfirmed
            {
                PhoneNumber = phone.Mobile.PhoneNumber,
                PhoneIDC = phone.Mobile.PhoneIDC,
                IsConfirmed = phone.Mobile.IsPhoneConfirmed
            };
        }
        else if (newCustomer is CustomerUpdateCustomerDetailWithChangesRequest contactsDetail)
        {
            contactsDetail.IsEmailConfirmed = email?.Email.IsEmailConfirmed ?? false;
            contactsDetail.IsPhoneConfirmed = phone?.Mobile.IsPhoneConfirmed ?? false;

            contactsDetail.EmailAddress = email is null ? null : new SharedTypesEmail
            {
                EmailAddress = email.Email.EmailAddress
            };
            contactsDetail.MobilePhone = phone is null ? null : new SharedTypesPhone
            {
                PhoneNumber = phone.Mobile.PhoneNumber,
                PhoneIDC = phone.Mobile.PhoneIDC
            };
        }

        return newCustomer;
    }

    private static Contact? GetPhone(CustomerDetailResponse customer)
    {
        var phone = customer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);

        return string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber) ? default : phone;
    }

    private static Contact? GetEmail(CustomerDetailResponse customer)
    {
        var email = customer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);

        return string.IsNullOrEmpty(email?.Email?.EmailAddress) ? default : email;
    }
}