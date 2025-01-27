﻿using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest.Incomes;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData.ProductRequest;

internal class Customer
{
    private readonly DomainServices.CustomerService.Contracts.Customer _customerDetail;

    private readonly ILookup<EnumIncomeTypes, IncomeInList> _customerIncomes;

    public Customer(CustomerOnSA customerOnSa, DomainServices.CustomerService.Contracts.Customer customerDetail)
    {
        CustomerOnSA = customerOnSa;
        _customerDetail = customerDetail;

        _customerIncomes = CustomerOnSA.Incomes.OrderBy(i => i.IncomeId).ToLookup(i => (EnumIncomeTypes)i.IncomeTypeId);
    }

    public required int HouseholdNumber { get; init; }

    public required bool IsPartner { get; init; }

    public required int FirstEmploymentTypeId { private get; init; }

    public required Dictionary<int, Income> Incomes { private get; init; }

    public required Dictionary<int, string> AcademicDegreesBefore { private get; init; }

    public required Dictionary<int, string> GenderCodes { private get; init; }

    public required ILookup<string, int> ObligationTypes { private get; init; }

    public required List<GenericCodebookResponse.Types.GenericCodebookItem> LegalCapacityTypes { private get; init; }

    public required List<BankCodesResponse.Types.BankCodeItem> BankCodes { private get; init; }

    public CustomerOnSA CustomerOnSA { get; }

    public bool? IsSpouseInDebt { get; init; }

    public Identity IdentityKb => CustomerOnSA.CustomerIdentifiers.GetKbIdentity();

    public Identity IdentityMp => CustomerOnSA.CustomerIdentifiers.GetMpIdentity();

    public bool HasRelationshipWithKB => CustomerOnSA.CustomerAdditionalData?.HasRelationshipWithKB ?? false;

    public bool HasRelationshipWithKBEmployee => CustomerOnSA.CustomerAdditionalData?.HasRelationshipWithKBEmployee ?? false;

    public bool HasRelationshipWithCorporate => CustomerOnSA.CustomerAdditionalData?.HasRelationshipWithCorporate ?? false;

    public bool IsPoliticallyExposed => CustomerOnSA.CustomerAdditionalData?.IsPoliticallyExposed ?? false;

    public bool IsUSPerson => CustomerOnSA.CustomerAdditionalData?.IsUSPerson ?? false;

    public string? RestrictionType => LegalCapacityTypes.Where(l => l.Id == CustomerOnSA.CustomerAdditionalData?.LegalCapacity?.RestrictionTypeId).Select(l => l.RdmCode).FirstOrDefault();

    public NaturalPerson NaturalPerson => _customerDetail.NaturalPerson;

    public IEnumerable<Address> Addresses => _customerDetail.Addresses.Select(a => new Address(a));

    public IEnumerable<CustomerContact> Contacts => _customerDetail.Contacts.Select(c => new CustomerContact(c));

    public IEnumerable<IdentificationDocument> IdentificationDocuments =>
        _customerDetail.IdentificationDocument != null
            ? new[] { _customerDetail.IdentificationDocument }
            : Enumerable.Empty<IdentificationDocument>();

    public string? DegreeBefore => NaturalPerson.DegreeBeforeId.HasValue ? AcademicDegreesBefore[NaturalPerson.DegreeBeforeId.Value] : default;

    public string GenderCode => GenderCodes[NaturalPerson.GenderId];

    public int? CitizenshipCountryId => NaturalPerson.CitizenshipCountriesId
                                                     .Cast<int?>()
                                                     .FirstOrDefault(id => id == 16, NaturalPerson.CitizenshipCountriesId.Cast<int?>().FirstOrDefault());

    public bool IsResident => NaturalPerson.TaxResidence?.ResidenceCountries.Any(r => r.CountryId == 16) ?? false;

    public IEnumerable<IncomeEmployment> IncomesEmployment => 
        _customerIncomes[EnumIncomeTypes.Employement].Select(i => new IncomeEmployment(i, Incomes[i.IncomeId])
        {
            Number = CustomerOnSA.Incomes.IndexOf(i),
            FirstEmploymentTypeId = FirstEmploymentTypeId
        });

    public IncomeEntrepreneur? IncomeEntrepreneur =>
        _customerIncomes[EnumIncomeTypes.Entrepreneur].Select(i => new IncomeEntrepreneur(i, Incomes[i.IncomeId])
        {
            Number = CustomerOnSA.Incomes.IndexOf(i)
        }).FirstOrDefault(); 
    
    public IncomeBase? IncomeRent =>
        _customerIncomes[EnumIncomeTypes.Rent].Select(i => new IncomeBase(i)
        {
            Number = CustomerOnSA.Incomes.IndexOf(i)
        }).FirstOrDefault();

    public IEnumerable<IncomeOther> IncomesOther =>
        _customerIncomes[EnumIncomeTypes.Other].Select(i => new IncomeOther(i, Incomes[i.IncomeId])
        {
            Number = CustomerOnSA.Incomes.IndexOf(i)
        });

    public IEnumerable<Obligation> Obligations =>
        CustomerOnSA.Obligations.Select((obligation, index) => new Obligation
        {
            Number = index + 1,
            ObligationData = obligation,
            ObligationTypeIds = ObligationTypes["amount"],
            BankCodes = BankCodes
        });

    public bool HasLockedIncomeDateTime => ((DateTime?)CustomerOnSA.LockedIncomeDateTime).HasValue;

    public DateTime? LockedIncomeDateTime => CustomerOnSA.LockedIncomeDateTime;
}