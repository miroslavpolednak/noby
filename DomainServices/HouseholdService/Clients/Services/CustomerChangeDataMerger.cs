using SharedTypes.Enums;
using SharedTypes.Types;
using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using Google.Protobuf.Collections;
using DomainServices.HouseholdService.Contracts.Model;

namespace DomainServices.HouseholdService.Clients.Services;

public class CustomerChangeDataMerger : ICustomerChangeDataMerger
{
    public void MergeAll(Customer customer, CustomerOnSA customerOnSA)
    {
        var customerChangeData = customerOnSA.GetCustomerChangeDataObject();

        if (customerChangeData is null)
            return;

        MergeClientData(customerChangeData, customer.NaturalPerson, customer.Addresses, customer.Contacts);

        customer.NaturalPerson.TaxResidence = MapDeltaToTaxResidence(customerChangeData.NaturalPerson?.TaxResidences) ?? customer.NaturalPerson.TaxResidence;
        customer.IdentificationDocument = MapDeltaToIdentificationDocument(customerChangeData.IdentificationDocument) ?? customer.IdentificationDocument;
        customer.CustomerIdentification = MapDeltaToCustomerIdentification(customerChangeData.CustomerIdentification) ?? customer.CustomerIdentification;
    }

    public void MergeClientData(Customer customer, CustomerOnSA customerOnSA)
    {
        var customerChangeData = customerOnSA.GetCustomerChangeDataObject();

        if (customerChangeData is null)
            return;

        MergeClientData(customerChangeData, customer.NaturalPerson, customer.Addresses, customer.Contacts);

        customer.IdentificationDocument = MapDeltaToIdentificationDocument(customerChangeData.IdentificationDocument) ?? customer.IdentificationDocument;
        customer.CustomerIdentification = MapDeltaToCustomerIdentification(customerChangeData.CustomerIdentification) ?? customer.CustomerIdentification;
    }

    public void MergeTaxResidence(NaturalPerson naturalPerson, CustomerOnSA customerOnSA)
    {
        var delta = customerOnSA.GetCustomerChangeDataObject()?.NaturalPerson?.TaxResidences;

        naturalPerson.TaxResidence = MapDeltaToTaxResidence(delta) ?? naturalPerson.TaxResidence;
    }

    private static void MergeClientData(CustomerChangeData customerChangeData, NaturalPerson naturalPerson, RepeatedField<GrpcAddress> addresses, RepeatedField<Contact> contacts)
    {
        RewriteWithDelta(naturalPerson, customerChangeData);
        RewriteWithDelta(addresses, customerChangeData.Addresses);
        RewriteWithDelta(contacts, customerChangeData.MobilePhone, customerChangeData.EmailAddress);
    }

    private static void RewriteWithDelta(NaturalPerson naturalPerson, CustomerChangeData customerChangeData)
    {
        var delta = customerChangeData.NaturalPerson;

        if (delta is null)
            return;

        naturalPerson.BirthNumber = delta.BirthNumber ?? naturalPerson.BirthNumber;
        naturalPerson.DegreeBeforeId = delta.DegreeBeforeId ?? naturalPerson.DegreeBeforeId;
        naturalPerson.FirstName = delta.FirstName ?? naturalPerson.FirstName;
        naturalPerson.LastName = delta.LastName ?? naturalPerson.LastName;
        naturalPerson.DateOfBirth = delta.DateOfBirth ?? naturalPerson.DateOfBirth;
        naturalPerson.BirthName = delta.BirthName ?? naturalPerson.BirthName;
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

        var confirmedContactAddress = addresses.FirstOrDefault(address => address is { AddressTypeId: (int)AddressTypes.Mailing, IsAddressConfirmed: true });
        
        addresses.Clear();

        //Add addresses from delta - it confirmed contact address exists then do not add mailing address
        addresses.AddRange(delta.Where(address => confirmedContactAddress is null || address.AddressTypeId != (int)AddressTypes.Mailing).Select(address => (GrpcAddress)address!));

        //Add confirmed contact address if exists
        if (confirmedContactAddress is not null)
            addresses.Add(confirmedContactAddress);
    }

    private static void RewriteWithDelta(RepeatedField<Contact> contacts, MobilePhoneDelta? mobileDelta, EmailAddressDelta? emailDelta)
    {
        if (mobileDelta is null && emailDelta is null)
            return;

        var mobilePhone = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil);
        var emailAddress = contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Email);

        contacts.Clear();

        if (mobilePhone?.Mobile.IsPhoneConfirmed != true && mobileDelta?.PhoneNumber != null)
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

        if (emailAddress?.Email.IsEmailConfirmed != true && emailDelta?.EmailAddress != null)
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
            }).OrderByDescending(t => t.CountryId == 16));
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

    private static CustomerIdentification? MapDeltaToCustomerIdentification(CustomerIdentificationDelta? delta)
    {
        if (delta is null)
            return default;

        return new CustomerIdentification
        {
            IdentificationMethodId = delta.IdentificationMethodId ?? 0,
            CzechIdentificationNumber = delta.CzechIdentificationNumber,
            IdentificationDate = delta.IdentificationDate
        };
    }
}