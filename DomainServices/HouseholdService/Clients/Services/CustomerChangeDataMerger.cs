using System.Text.Json;
using CIS.Foms.Enums;
using CIS.Foms.Types;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Contracts.Dto;
using Google.Protobuf.Collections;

namespace DomainServices.HouseholdService.Clients.Services;

public class CustomerChangeDataMerger : ICustomerChangeDataMerger
{
    public void Merge(CustomerDetailResponse customer, CustomerOnSA customerOnSA)
    {
        if (string.IsNullOrWhiteSpace(customerOnSA.CustomerChangeData))
            return;

        var delta = JsonSerializer.Deserialize<CustomerChangeDataDelta>(customerOnSA.CustomerChangeData);

        if (delta is null)
            return;

        RewriteWithDelta(customer.NaturalPerson, delta.NaturalPerson);
        RewriteWithDelta(customer.Addresses, delta.Addresses);
        RewriteWithDelta(customer.Contacts, delta.MobilePhone, delta.EmailAddress);

        customer.NaturalPerson.TaxResidence = MapDeltaToTaxResidence(delta.NaturalPerson?.TaxResidences) ?? customer.NaturalPerson.TaxResidence;
        customer.IdentificationDocument = MapDeltaToIdentificationDocument(delta.IdentificationDocument) ?? customer.IdentificationDocument;
    }

    private static void RewriteWithDelta(NaturalPerson naturalPerson, NaturalPersonDelta? delta)
    {
        if (delta is null)
            return;

        naturalPerson.BirthNumber = delta.BirthNumber ?? naturalPerson.BirthNumber;
        naturalPerson.DegreeBeforeId = delta.DegreeBeforeId ?? naturalPerson.DegreeBeforeId;
        naturalPerson.FirstName = delta.FirstName ?? naturalPerson.FirstName;
        naturalPerson.LastName = delta.LastName ?? naturalPerson.LastName;
        naturalPerson.DateOfBirth = delta.DateOfBirth ?? naturalPerson.DateOfBirth;
        naturalPerson.PlaceOfBirth = delta.PlaceOfBirth ?? naturalPerson.PlaceOfBirth;
        naturalPerson.BirthCountryId = delta.BirthCountryId ?? naturalPerson.BirthCountryId;
        naturalPerson.GenderId = delta.Gender != Genders.Unknown ? (int)delta.Gender : naturalPerson.GenderId;
        naturalPerson.MaritalStatusStateId = delta.MaritalStatusId ?? naturalPerson.MaritalStatusStateId;
        naturalPerson.EducationLevelId = delta.EducationLevelId ?? naturalPerson.EducationLevelId;
        naturalPerson.ProfessionCategoryId = delta.ProfessionCategoryId ?? naturalPerson.ProfessionCategoryId;
        naturalPerson.ProfessionId = delta.ProfessionId ?? naturalPerson.ProfessionId;
        naturalPerson.NetMonthEarningAmountId = delta.NetMonthEarningAmountId ?? naturalPerson.NetMonthEarningAmountId;
        naturalPerson.NetMonthEarningTypeId = delta.NetMonthEarningTypeId ?? naturalPerson.NetMonthEarningTypeId;

        if (delta.CitizenshipCountriesId is not null)
        {
            naturalPerson.CitizenshipCountriesId.Clear();
            naturalPerson.CitizenshipCountriesId.AddRange(delta.CitizenshipCountriesId);
        }
    }

    private static void RewriteWithDelta(RepeatedField<GrpcAddress> addresses, IEnumerable<Address>? delta)
    {
        if (delta is null)
            return;

        addresses.Clear();
        addresses.AddRange(delta.Select(address => (GrpcAddress)address!));
    }

    private static void RewriteWithDelta(RepeatedField<Contact> contacts, MobilePhoneDelta? mobileDelta, EmailAddressDelta? emailDelta)
    {
        if (mobileDelta is null && emailDelta is null)
            return;

        var mobilePhone = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);
        var emailAddress = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        contacts.Clear();

        if (mobileDelta?.PhoneNumber != null)
        {
            mobilePhone = new Contact
            {
                IsPrimary = true,
                ContactTypeId = (int)ContactTypes.Mobil,
                Mobile = new MobilePhoneItem
                {
                    IsPhoneConfirmed = false,
                    PhoneNumber = mobileDelta.PhoneNumber,
                    PhoneIDC = mobileDelta.PhoneIDC ?? string.Empty
                }
            };
        }

        if (emailDelta?.EmailAddress != null)
        {
            emailAddress = new Contact
            {
                IsPrimary = true,
                ContactTypeId = (int)ContactTypes.Email,
                Email = new EmailAddressItem { EmailAddress = emailDelta.EmailAddress }
            };
        }

        if (mobilePhone is not null)
            contacts.Add(mobilePhone);

        if (emailAddress is not null)
            contacts.Add(emailAddress);
    }

    private static NaturalPersonTaxResidence? MapDeltaToTaxResidence(NaturalPersonDelta.TaxResidenceDelta? delta)
    {
        if (delta is null)
            return default;

        var taxResidence = new NaturalPersonTaxResidence
        {
            ValidFrom = delta.ValidFrom
        };

        if (delta.ResidenceCountries is not null)
        {
            taxResidence.ResidenceCountries.AddRange(delta.ResidenceCountries.Select(r => new NaturalPersonResidenceCountry
            {
                CountryId = r.CountryId,
                Tin = r.Tin,
                TinMissingReasonDescription = r.TinMissingReasonDescription
            }));
        }

        return taxResidence;
    }

    private static IdentificationDocument? MapDeltaToIdentificationDocument(IdentificationDocumentDelta? delta)
    {
        if (delta is null)
            return default;

        return new IdentificationDocument
        {
            IdentificationDocumentTypeId = delta.IdentificationDocumentTypeId,
            ValidTo = delta.ValidTo,
            Number = delta.Number,
            IssuedBy = delta.IssuedBy,
            IssuingCountryId = delta.IssuingCountryId,
            IssuedOn = delta.IssuedOn,
            RegisterPlace = delta.RegisterPlace
        };
    }
}