using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.Incomes;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

internal class Customer
{
    private readonly CustomerOnSA _customerOnSa;
    private readonly CustomerDetailResponse _customerDetail;

    private readonly ILookup<CustomerIncomeTypes, IncomeInList> _customerIncomes;

    public Customer(CustomerOnSA customerOnSa, CustomerDetailResponse customerDetail)
    {
        _customerOnSa = customerOnSa;
        _customerDetail = customerDetail;

        _customerIncomes = _customerOnSa.Incomes.OrderBy(i => i.IncomeId).ToLookup(i => (CustomerIncomeTypes)i.IncomeTypeId);
    }
    public required int HouseholdNumber { get; init; }

    public required bool IsPartner { get; init; }

    public required int FirstEmploymentTypeId { private get; init; }

    public required Dictionary<int, Income> Incomes { private get; init; }

    public required Dictionary<int, string> AcademicDegreesBefore { private get; init; }

    public required Dictionary<int, string> GenderCodes { private get; init; }

    public required Dictionary<int, string> Countries { private get; init; }

    public required ILookup<string, int> ObligationTypes { private get; init; }

    public int RoleId => _customerOnSa.CustomerRoleId;

    public Identity IdentityKb => _customerOnSa.CustomerIdentifiers.Single(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

    public Identity IdentityMp => _customerOnSa.CustomerIdentifiers.Single(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

    public bool HasRelationshipWithKB => _customerOnSa.CustomerAdditionalData?.HasRelationshipWithKB ?? false;

    public bool HasRelationshipWithKBEmployee => _customerOnSa.CustomerAdditionalData?.HasRelationshipWithKBEmployee ?? false;

    public bool HasRelationshipWithCorporate => _customerOnSa.CustomerAdditionalData?.HasRelationshipWithCorporate ?? false;

    public NaturalPerson NaturalPerson => _customerDetail.NaturalPerson;

    public bool IsPoliticallyExposed => _customerDetail.NaturalPerson.IsPoliticallyExposed ?? false;

    public IEnumerable<Address> Addresses => _customerDetail.Addresses.Select(a => new Address(a));

    public IEnumerable<Contact> Contacts => _customerDetail.Contacts;

    public IEnumerable<IdentificationDocument> IdentificationDocuments =>
        _customerDetail.IdentificationDocument != null
            ? new[] { _customerDetail.IdentificationDocument }
            : Enumerable.Empty<IdentificationDocument>();

    public string? DegreeBefore => NaturalPerson.DegreeBeforeId.HasValue ? AcademicDegreesBefore[NaturalPerson.DegreeBeforeId.Value] : default;

    public string GenderCode => GenderCodes[NaturalPerson.GenderId];

    public int? CitizenshipCountryId => NaturalPerson.CitizenshipCountriesId.Select(id => (int?)id).FirstOrDefault();

    public bool IsResident => NaturalPerson.TaxResidence?.ResidenceCountries?.Any() ?? false && Countries[NaturalPerson.TaxResidence.ResidenceCountries.First().CountryId.GetValueOrDefault()] == "CZ";

    public int DefaultZeroValue => 0;

    public IEnumerable<IncomeEmployment> IncomesEmployment => 
        _customerIncomes[CustomerIncomeTypes.Employement].Select(i => new IncomeEmployment(i, Incomes[i.IncomeId])
        {
            Number = _customerOnSa.Incomes.IndexOf(i),
            FirstEmploymentTypeId = FirstEmploymentTypeId
        });

    public IncomeEntrepreneur? IncomeEntrepreneur =>
        _customerIncomes[CustomerIncomeTypes.Enterprise].Select(i => new IncomeEntrepreneur(i, Incomes[i.IncomeId])
        {
            Number = _customerOnSa.Incomes.IndexOf(i)
        }).FirstOrDefault(); 
    
    public IncomeBase? IncomeRent =>
        _customerIncomes[CustomerIncomeTypes.Rent].Select(i => new IncomeBase(i)
        {
            Number = _customerOnSa.Incomes.IndexOf(i)
        }).FirstOrDefault();

    public IEnumerable<IncomeOther> IncomesOther =>
        _customerIncomes[CustomerIncomeTypes.Other].Select(i => new IncomeOther(i, Incomes[i.IncomeId])
        {
            Number = _customerOnSa.Incomes.IndexOf(i)
        });

    public IEnumerable<Obligation> Obligations =>
        _customerOnSa.Obligations.Select((obligation, index) => new Obligation
        {
            Number = index + 1,
            ObligationData = obligation,
            ObligationTypeIds = ObligationTypes["amount"]
        });

    public bool HasLockedIncomeDateTime => ((DateTime?)_customerOnSa.LockedIncomeDateTime).HasValue;

    public DateTime? LockedIncomeDateTime => _customerOnSa.LockedIncomeDateTime;
}