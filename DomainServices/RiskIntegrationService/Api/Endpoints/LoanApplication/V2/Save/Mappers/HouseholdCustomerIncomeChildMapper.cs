using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using CIS.Core;
using DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdCustomerIncomeChildMapper
{
    public async Task<_C4M.LoanApplicationIncome?> MapIncomes(_V2.LoanApplicationIncome? income, bool verification)
    {
        if (income is null) return null;

        var model = new _C4M.LoanApplicationIncome
        {
            IncomeConfirmed = income.IsIncomeConfirmed,
            LastConfirmedDate = income.LastConfirmedDate ?? System.DateTime.MinValue,
            EmploymentIncomes = await mapIncomesEmployment(income.EmploymentIncomes, verification),
            EntrepreneurIncome = await mapIncomesEntrepreneur(income.EntrepreneurIncome),
            RentIncome = await mapIncomesRent(income.RentIncome),
            OtherIncomes = await mapIncomesOther(income.OtherIncomes)
        };

        model.IncomeCollected = model.EntrepreneurIncome is not null || model.EmploymentIncomes is not null || model.RentIncome is not null || model.OtherIncomes is not null;

        return model;
    }

    private async Task<List<_C4M.LoanApplicationEmploymentIncome>?> mapIncomesEmployment(List<_V2.LoanApplicationEmploymentIncome>? income, bool verification)
    {
        if (income is null) return null;

        return (await income.SelectAsync(async t => new _C4M.LoanApplicationEmploymentIncome
            {
                EmployerIdentificationNumber = t.EmployerIdentificationNumber,
                EmployerName = t.EmployerName,
                EmployerCountryCode = (await _codebookService.Countries(_cancellationToken)).FirstOrDefault(x => x.Id == t.Address?.CountryId)?.ShortName,
                JobTitle = t.JobDescription,
                AccountantContacts = string.IsNullOrEmpty(t.PhoneNumber) ? null : new List<_C4M.Contact> { new _C4M.Contact { ContactCategory = _C4M.ContactCategoryType.BUSINESS, ContactType = _C4M.ContactType.PHONE, Value = t.PhoneNumber } },
                Domiciled = t.IsDomicile.ToInt(),
                ProofType = await getProofType<_C4M.ProofType>(t.ProofTypeId),
                ProofMonthlyIncomeAmount = t.MonthlyIncomeAmount is null ? null : t.MonthlyIncomeAmount!.Amount.ToAmount(),
                ForeignEmploymentTypeCode = (await _codebookService.IncomeForeignTypes(_cancellationToken)).FirstOrDefault(x => x.Id == t.IncomeForeignTypeId)?.Code,
                ProofConfirmationContactPhone = t.ConfirmationContactPhone,
                ProofConfirmationContactSurname = t.ConfirmationPerson,
                ProofCreatedOn = t.ConfirmationDate,
                ProbationaryPeriod = t.JobTrialPeriod,
                NoticePeriod = t.NoticePeriod,
                EmploymentTypeCode = (await _codebookService.EmploymentTypes(_cancellationToken)).FirstOrDefault(x => x.Id == t.EmploymentTypeId)?.Code,
                FirstContractFrom = t.FirstWorkContractSince,
                DateRange = t.CurrentWorkContractSince != null || t.CurrentWorkContractTo != null ? new _C4M.DateRange { DateFrom = t.CurrentWorkContractSince, DateTo = t.CurrentWorkContractTo} : null,
                VerificationPriority = verification.ToInt(),
                IssuedByExternalAccountant = t.ConfirmationByCompany,
                IncomeDeduction = getDeductions(t.IncomeDeduction)
        }))
        .ToList();
    }

    private async Task<_C4M.LoanApplicationEntrepreneurIncome?> mapIncomesEntrepreneur(_V2.LoanApplicationEntrepreneurIncome? income)
    {
        if (income is null) return null;

        return new _C4M.LoanApplicationEntrepreneurIncome
        {
            EntrepreneurIdentificationNumber = income.EntrepreneurIdentificationNumber,
            CountryCode = (await _codebookService.Countries(_cancellationToken)).FirstOrDefault(t => t.Id == income.Address?.CountryId)?.ShortName,
            DateRange = income.EstablishedOn != null ? new _C4M.DateRange { DateFrom = income.EstablishedOn } : null,
            Domiciled = income.IsDomicile.ToInt(),
            ProofType = await getProofType<_C4M.ProofType>(income.ProofTypeId),
            DeclaredIncome = income.AnnualIncomeAmount.ToAmount(),
            DeclaredIncomePeriod = _C4M.IncomePeriodType.YEAR,
            LumpSumModified = income.LumpSumModified,
            LumpSumTaxationRegime = income.LumpSumTaxationRegime
        };
    }

    private async Task<_C4M.LoanApplicationRentIncome?> mapIncomesRent(_V2.LoanApplicationRentIncome? income)
        => income is null ? null : new _C4M.LoanApplicationRentIncome
        {
            Domiciled = income.IsDomicile.ToInt(),
            DeclaredIncome = income.MonthlyIncomeAmount.ToAmount(),
            DeclaredIncomePeriod = _C4M.IncomePeriodType.MONTH,
            ProofType = await getProofType<_C4M.ProofType>(income.ProofTypeId)
        };

    private async Task<List<_C4M.LoanApplicationOtherIncome>?> mapIncomesOther(List<_V2.LoanApplicationOtherIncome>? incomes)
        => incomes is null ? null : (await incomes.SelectAsync(async income => new _C4M.LoanApplicationOtherIncome
        {
            Domiciled = income.IsDomicile.ToInt(),
            DeclaredIncome = income.MonthlyIncomeAmount.ToAmount(),
            DeclaredIncomePeriod = _C4M.IncomePeriodType.MONTH,
            OtherIncomeTypeCode = (await _codebookService.IncomeOtherTypes(_cancellationToken)).FirstOrDefault(x => x.Id == income.IncomeOtherTypeId)?.Code,
            ProofType = await getProofType<_C4M.ProofType>(income.ProofTypeId)
        })).ToList();

    private static List<_C4M.IncomeDeduction> getDeductions(_V2.LoanApplicationEmploymentIncomeDeduction? deduction)
        => new ()
        {
            new _C4M.IncomeDeduction { Type = _C4M.IncomeDeductionType.EXECUTION, Amount = (deduction?.Execution).ToAmountDefault() },
            new _C4M.IncomeDeduction { Type = _C4M.IncomeDeductionType.INSTALLMENTS, Amount = (deduction?.Installments).ToAmountDefault() },
            new _C4M.IncomeDeduction { Type = _C4M.IncomeDeductionType.OTHER, Amount = (deduction?.Other).ToAmountDefault() }
        };

    private async Task<TResponse?> getProofType<TResponse>(int? proofTypeId) where TResponse : struct
        => Helpers.GetEnumFromString<TResponse>((await _codebookService.ProofTypes(_cancellationToken)).FirstOrDefault(t => t.Id == proofTypeId)?.Code);

    private static long? getZipCode(string? zip)
    {
        long code;
        return long.TryParse(zip, out code) ? code : null;
    }

    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly CancellationToken _cancellationToken;

    public HouseholdCustomerIncomeChildMapper(
        CodebookService.Clients.ICodebookServiceClient codebookService,
        CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
