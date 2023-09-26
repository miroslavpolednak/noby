﻿using SharedTypes.Enums;
using SharedTypes.Types;
using SharedTypes.GrpcTypes;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using DomainServices.HouseholdService.Contracts.Dto;
using Google.Protobuf.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DomainServices.HouseholdService.Clients.Services;

public class CustomerChangeDataMerger : ICustomerChangeDataMerger
{

    public JsonSerializerOptions JsonSerializationOptions { get; set; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    /// <summary>
    /// Throw away locally stored CRS (TaxResidences) data and keep client changes
    /// </summary>
    /// <returns>json string</returns>
    public string? TrowAwayLocallyStoredCrsData(CustomerOnSA customerOnSA)
    {
        var currentDelta = GetCustomerChangeDataDelta(customerOnSA);
        var taxResidences = currentDelta?.NaturalPerson?.TaxResidences;
        if (taxResidences is not null)
        {
            currentDelta!.NaturalPerson!.TaxResidences = null;
        }

        return JsonSerializer.Serialize(currentDelta, JsonSerializationOptions);
    }

    /// <summary>
    /// Throw away locally stored Client data (keep CRS changes)
    /// </summary>
    /// <returns>json string</returns>
    public string? TrowAwayLocallyStoredClientData(CustomerOnSA customerOnSA)
    {
        var currentDelta = GetCustomerChangeDataDelta(customerOnSA);
        var currentDeltaTaxResidences = currentDelta?.NaturalPerson?.TaxResidences;

        if (currentDeltaTaxResidences is not null)
        {
            var deltaWithCrsOnly = new CustomerChangeDataDelta
            {
                NaturalPerson = new()
                {
                    TaxResidences = new()
                    {
                        ValidFrom = currentDeltaTaxResidences.ValidFrom,
                        ResidenceCountries = currentDeltaTaxResidences.ResidenceCountries is not null && currentDeltaTaxResidences.ResidenceCountries.Any()
                            ? new(currentDeltaTaxResidences.ResidenceCountries!)
                            : new()
                    }
                }
            };

            return JsonSerializer.Serialize(deltaWithCrsOnly, JsonSerializationOptions);
        }
        else
        {
            return default;
        }
    }

    public void MergeAll(CustomerDetailResponse customer, CustomerOnSA customerOnSA)
    {
        var delta = GetCustomerChangeDataDelta(customerOnSA);

        if (delta is null)
            return;

        MergeClientData(delta, customer.NaturalPerson, customer.Addresses, customer.Contacts);

        customer.NaturalPerson.TaxResidence = MapDeltaToTaxResidence(delta.NaturalPerson?.TaxResidences) ?? customer.NaturalPerson.TaxResidence;
        customer.IdentificationDocument = MapDeltaToIdentificationDocument(delta.IdentificationDocument) ?? customer.IdentificationDocument;
    }

    public void MergeClientData(CustomerDetailResponse customer, CustomerOnSA customerOnSA)
    {
        var delta = GetCustomerChangeDataDelta(customerOnSA);

        if (delta is null)
            return;

        MergeClientData(delta, customer.NaturalPerson, customer.Addresses, customer.Contacts);

        customer.IdentificationDocument = MapDeltaToIdentificationDocument(delta.IdentificationDocument) ?? customer.IdentificationDocument;
        customer.CustomerIdentification = MapDeltaToCustomerIdentification(delta.CustomerIdentification) ?? customer.CustomerIdentification;
    }

    public void MergeTaxResidence(NaturalPerson naturalPerson, CustomerOnSA customerOnSA)
    {
        var delta = GetCustomerChangeDataDelta(customerOnSA)?.NaturalPerson?.TaxResidences;

        naturalPerson.TaxResidence = MapDeltaToTaxResidence(delta) ?? naturalPerson.TaxResidence;
    }

    private static CustomerChangeDataDelta? GetCustomerChangeDataDelta(CustomerOnSA customerOnSA)
    {
        if (string.IsNullOrWhiteSpace(customerOnSA.CustomerChangeData))
            return default;

        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<CustomerChangeDataDelta>(customerOnSA.CustomerChangeData, options);
    }

    private static void MergeClientData(CustomerChangeDataDelta delta, NaturalPerson naturalPerson, RepeatedField<GrpcAddress> addresses, RepeatedField<Contact> contacts)
    {
        RewriteWithDelta(naturalPerson, delta.NaturalPerson);
        RewriteWithDelta(addresses, delta.Addresses);
        RewriteWithDelta(contacts, delta.MobilePhone, delta.EmailAddress);
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