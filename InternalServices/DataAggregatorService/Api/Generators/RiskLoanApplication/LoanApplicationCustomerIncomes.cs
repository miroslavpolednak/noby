using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

public class LoanApplicationCustomerIncomes
{
    private readonly CustomerOnSA _customerOnSA;
    private readonly IReadOnlyDictionary<int, Income> _incomes;
    private readonly ILookup<EnumIncomeTypes, IncomeInList> _customerIncomes;

    public LoanApplicationCustomerIncomes(CustomerOnSA customerOnSA, IReadOnlyDictionary<int, Income> incomes)
    {
        _customerOnSA = customerOnSA;
        _incomes = incomes;

        _customerIncomes = customerOnSA.Incomes.ToLookup(i => (EnumIncomeTypes)i.IncomeTypeId);

        Employments = GetEmploymentIncomes();
        Entrepreneur = GetEntrepreneurIncome();
        Rent = _customerIncomes[EnumIncomeTypes.Rent].FirstOrDefault();
        Others = GetOtherIncomes();
    }

    public bool IsIncomeConfirmed => _customerOnSA.LockedIncomeDateTime is not null;

    public DateTime LastConfirmedDate => (DateTime?)_customerOnSA.LockedIncomeDateTime ?? default;

    public List<EmploymentIncomes> Employments { get; }

    public EntrepreneurIncome? Entrepreneur { get; }

    public IncomeInList? Rent { get; }

    public List<OtherIncome> Others { get; }

    private List<EmploymentIncomes> GetEmploymentIncomes() => 
        _customerIncomes[EnumIncomeTypes.Employement].Select(incomeInList => new EmploymentIncomes(incomeInList, _incomes[incomeInList.IncomeId])).ToList();

    private EntrepreneurIncome? GetEntrepreneurIncome() => 
        _customerIncomes[EnumIncomeTypes.Entrepreneur].Select(incomeInList => new EntrepreneurIncome(incomeInList, _incomes[incomeInList.IncomeId])).FirstOrDefault();

    private List<OtherIncome> GetOtherIncomes() =>
        _customerIncomes[EnumIncomeTypes.Other].Select(incomeInList => new OtherIncome(incomeInList, _incomes[incomeInList.IncomeId])).ToList();

    public class EmploymentIncomes
    {
        private readonly IncomeInList _incomeInList;

        public EmploymentIncomes(IncomeInList incomeInList, Income income)
        {
            _incomeInList = incomeInList;
            Income = income.Employement;
        }

        public IncomeDataEmployement? Income { get; }

        public decimal Sum => (decimal?)_incomeInList.Sum ?? default;

        public int ProofTypeId => 6;

        public string? EmployerIdentificationNumber => new[] { Income?.Employer?.Cin, Income?.Employer?.BirthNumber }.FirstOrDefault(str => !string.IsNullOrWhiteSpace(str));

        public string EmployerName => Income?.Employer?.Name ?? string.Empty;

        public string? IncomeConfirmationContact
        {
            get
            {
                var contact = Income?.IncomeConfirmation?.ConfirmationContact;

                return contact is null ? default : $"{contact.PhoneIDC}{contact.PhoneNumber}";
            }
        }
    }

    public class EntrepreneurIncome
    {
        private readonly IncomeInList _incomeInList;

        public EntrepreneurIncome(IncomeInList incomeInList, Income income)
        {
            _incomeInList = incomeInList;
            Income = income.Entrepreneur;
        }

        public IncomeDataEntrepreneur? Income { get; }

        public string? EntrepreneurIdentificationNumber => new[] { Income?.Cin, Income?.BirthNumber }.FirstOrDefault(str => !string.IsNullOrWhiteSpace(str));

        public int ProofTypeId => 2;

        public decimal Sum => (decimal?)_incomeInList.Sum ?? default;
    }

    public class OtherIncome
    {
        private readonly IncomeInList _incomeInList;
        private readonly Income _income;

        public OtherIncome(IncomeInList incomeInList, Income income)
        {
            _incomeInList = incomeInList;
            _income = income;
        }

        public decimal Sum => (decimal?)_incomeInList.Sum ?? default;

        public int IncomeOtherTypeId => _income.Other?.IncomeOtherTypeId ?? default;
    }
}