using System.Globalization;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

internal class LoanApplicationCustomer
{
    private readonly CustomerOnSA _customerOnSA;
    private readonly List<GenericCodebookResponse.Types.GenericCodebookItem> _degreesBefore;

    public LoanApplicationCustomer(CustomerOnSA customerOnSA,
                                   CustomerDetailResponse customerDetail,
                                   IReadOnlyDictionary<int, Income> incomes,
                                   List<GenericCodebookResponse.Types.GenericCodebookItem> degreesBefore)
    {
        _customerOnSA = customerOnSA;
        _degreesBefore = degreesBefore;
        CustomerDetail = customerDetail;

        PermanentAddress = GetPermanentAddress();
        Incomes = new LoanApplicationCustomerIncomes(customerOnSA, incomes);
    }

    public CustomerDetailResponse CustomerDetail { get; }

    public GrpcAddress? PermanentAddress { get; }

    public List<LoanApplicationObligation> Obligations { get; private init; } = null!;

    public LoanApplicationCustomerIncomes Incomes { get; }

    public required bool IsPartner { get; init; }

    public required IReadOnlyDictionary<string, List<int>> ObligationTypes
    {
        init
        {
            Obligations = _customerOnSA.Obligations.Where(o => o.Creditor?.IsExternal == true)
                                       .Select(o => new LoanApplicationObligation(o, value.GetValueOrDefault("amount") ?? new List<int>()))
                                       .ToList();
        }
    }

    public int InternalCustomerId => _customerOnSA.CustomerOnSAId;

    public string CustomerId => _customerOnSA.CustomerIdentifiers.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId.ToString(CultureInfo.InvariantCulture);

    public int CustomerRoleId => _customerOnSA.CustomerRoleId;

    public bool SpecialRelationsWithKB => _customerOnSA.CustomerAdditionalData?.HasRelationshipWithKB ?? false;

    public int? GenderId => CustomerDetail.NaturalPerson.GenderId == (int)Genders.Unknown ? null : CustomerDetail.NaturalPerson.GenderId;

    public int? EducationLevelId => CustomerDetail.NaturalPerson.EducationLevelId > 0 ? CustomerDetail.NaturalPerson.EducationLevelId : null;

    public string? DegreeBefore => CustomerDetail.NaturalPerson.DegreeBeforeId.HasValue ? _degreesBefore.First(d => d.Id == CustomerDetail.NaturalPerson.DegreeBeforeId.Value).Name : default;

    public string PhoneNumber
    {
        get
        {
            var phoneNumber = CustomerDetail.Contacts.FirstOrDefault(c => c.ContactTypeId == (int)ContactTypes.Mobil && c.IsPrimary);

            return phoneNumber is null ? string.Empty : $"{phoneNumber.Mobile.PhoneIDC}{phoneNumber.Mobile.PhoneNumber}";
        }
    }

    public bool HasEmail => CustomerDetail.Contacts.Any(c => c.ContactTypeId == (int)ContactTypes.Email && c.IsPrimary);

    public bool IsTaxPayer => CustomerDetail.NaturalPerson.TaxResidence?.ResidenceCountries.Any(t => t.CountryId == 16) ?? false;

    private GrpcAddress? GetPermanentAddress()
    {
        var address = CustomerDetail.Addresses.FirstOrDefault(a => a.AddressTypeId == (int)AddressTypes.Permanent);

        if (address is null)
            return default;

        address.Postcode = address.Postcode.Replace(" ", "");

        return address;
    }
}