using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Contracts.Model;
using NOBY.Dto.Customer;
using NOBY.Dto;

namespace NOBY.Services.Customer;

public static class CustomerMapper
{
    public static CustomerChangeData? MapCustomerDtoToChangeData<TCustomerDetail>(TCustomerDetail? customerDto) where TCustomerDetail : BaseCustomerDetail
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
                Gender = customerDto.NaturalPerson.Gender,
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
                    ValidFrom = customerDto.NaturalPerson.TaxResidences.validFrom,
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
                ValidTo = customerDto.IdentificationDocument.ValidTo,
                IssuedOn = customerDto.IdentificationDocument.IssuedOn,
                RegisterPlace = customerDto.IdentificationDocument.RegisterPlace,
                Number = customerDto.IdentificationDocument.Number
            },
            Addresses = customerDto.Addresses?.ToList(),
        };

        if (customerDto is ICustomerDetailConfirmedContacts confirmedContactsDetail)
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

        if (customerDto is ICustomerDetailContacts contactsDetail)
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

    public static TCustomerDetail MapCustomerToResponseDto<TCustomerDetail>(DomainServices.CustomerService.Contracts.CustomerDetailResponse dsCustomer, CustomerOnSA customerOnSA) where TCustomerDetail : BaseCustomerDetail
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
        newCustomer.Addresses = dsCustomer.Addresses?.Select(t => (SharedTypes.Types.Address)t!).ToList();

        // https://jira.kb.cz/browse/HFICH-4200
        // docasne reseni nez se CM rozmysli jak na to
        newCustomer.LegalCapacity = new LegalCapacityItem
        {
            RestrictionTypeId = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId,
            RestrictionUntil = customerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionUntil
        };

        if (newCustomer is ICustomerDetailConfirmedContacts confirmedContactsDetail)
        {
            confirmedContactsDetail.EmailAddress = getEmail<EmailAddressConfirmedDto>(dsCustomer);
            confirmedContactsDetail.MobilePhone = getPhone<PhoneNumberConfirmedDto>(dsCustomer);
        }
        else if (newCustomer is ICustomerDetailContacts contactsDetail)
        {
            contactsDetail.EmailAddress = getEmail<EmailAddressDto>(dsCustomer);
            contactsDetail.MobilePhone = getPhone<PhoneNumberDto>(dsCustomer);
        }

        return newCustomer;
    }

    private static TPhone? getPhone<TPhone>(DomainServices.CustomerService.Contracts.CustomerDetailResponse customer) where TPhone : IPhoneNumberDto
    {
        var phone = customer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Mobil);
        if (string.IsNullOrEmpty(phone?.Mobile?.PhoneNumber)) 
            return default;

        var newPhone = (TPhone)Activator.CreateInstance(typeof(TPhone))!;
        newPhone.IsConfirmed = phone.Mobile.IsPhoneConfirmed;
        newPhone.PhoneNumber = phone.Mobile.PhoneNumber;
        newPhone.PhoneIDC = phone.Mobile.PhoneIDC;

        return newPhone;
    }

    private static TEmail? getEmail<TEmail>(DomainServices.CustomerService.Contracts.CustomerDetailResponse customer) where TEmail : IEmailAddressDto
    {
        var email = customer.Contacts.FirstOrDefault(t => t.ContactTypeId == (int)ContactTypes.Email);
        if (string.IsNullOrEmpty(email?.Email?.EmailAddress)) 
            return default;

        var newEmail = (TEmail)Activator.CreateInstance(typeof(TEmail))!;
        newEmail.IsConfirmed = email.Email.IsEmailConfirmed;
        newEmail.EmailAddress = email.Email.EmailAddress;

        return newEmail;

    }
}