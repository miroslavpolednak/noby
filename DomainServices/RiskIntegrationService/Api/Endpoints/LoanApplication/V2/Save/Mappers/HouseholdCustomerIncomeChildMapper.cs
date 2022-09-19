using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using CIS.Core;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdCustomerIncomeChildMapper
{
    public async Task<_C4M.LoanApplicationIncome?> MapIncomes(_V2.LoanApplicationIncome? income, bool verification)
    {
        if (income is null) return null;

        var model = new _C4M.LoanApplicationIncome
        {
            IncomeConfirmed = income.IsIncomeConfirmed,
            LastConfirmedDate = income.LastConfirmedDate,
            EmploymentIncome = await mapIncomesEmployment(income.EmploymentIncomes, verification),
            EntrepreneurIncome = await mapIncomesEntrepreneur(income.EntrepreneurIncome),
            RentIncome = await mapIncomesRent(income.RentIncome),
            OtherIncome = await mapIncomesOther(income.OtherIncomes)
        };

        model.IncomeCollected = model.EntrepreneurIncome is not null || model.EmploymentIncome is not null || model.RentIncome is not null || model.OtherIncome is not null;

        return model;
    }

    private async Task<List<_C4M.LoanApplicationEmploymentIncome>?> mapIncomesEmployment(List<_V2.LoanApplicationEmploymentIncome>? income, bool verification)
    {
        if (income is null) return null;

        return (await income.SelectAsync(async t => new _C4M.LoanApplicationEmploymentIncome
            {
                EmployerIdentificationNumber = t.EmployerIdentificationNumber,
                EmployerType = t.WorkSectorId,
                EmployerName = t.EmployerName,
                Nace = t.ClassficationOfEconomicActivityId,
                Profession = t.JobTypeId,
                Street = t.Address?.Street,
                HouseNumber = t.Address?.BuildingIdentificationNumber,
                StreetNumber = t.Address?.LandRegistryNumber,
                Postcode = getZipCode(t.Address?.Postcode),//TODO c4m predela na string
                City = t.Address?.City,
                CountryCode = (await _codebookService.Countries(_cancellationToken)).FirstOrDefault(x => x.Id == t.Address?.CountryId)?.ShortName,
                JobTitle = t.JobDescription,
                Phone = string.IsNullOrEmpty(t.PhoneNumber) ? null : new PhoneContact { ContactType = PhoneContactContactType.BUSINESS, PhoneNumber = t.PhoneNumber },
                AccountNumber = t.BankAccount?.ConvertToString(),
                Domiciled = t.IsDomicile,
                ProofType = await getProofType<LoanApplicationEmploymentIncomeProofType>(t.ProofTypeId),
                MonthlyIncomeAmount = t.MonthlyIncomeAmount is null ? null : t.MonthlyIncomeAmount!.Amount.ToAmount(),
                ForeignEmploymentType = (await _codebookService.IncomeForeignTypes(_cancellationToken)).FirstOrDefault(x => x.Id == t.IncomeForeignTypeId)?.Code,
                GrossAnnualIncome = t.GrossAnnualIncome.HasValue ? Convert.ToDouble(t.GrossAnnualIncome!, System.Globalization.CultureInfo.InvariantCulture) : null,
                ProofConfirmationContactPhone = t.ConfirmationContactPhone,
                ProofConfirmationContactSurname = t.ConfirmationPerson,
                ProofCreatedOn = t.ConfirmationDate,
                ProbationaryPeriod = t.JobTrialPeriod,
                NoticePeriod = t.NoticePeriod,
                EmploymentType = (await _codebookService.EmploymentTypes(_cancellationToken)).FirstOrDefault(x => x.Id == t.EmploymentTypeId)?.Code,
                FirstContractFrom = t.FirstWorkContractSince,
                CurrentContractFrom = t.CurrentWorkContractSince,
                CurrentContractTo = t.CurrentWorkContractTo,
                VerificationPriority = verification,
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
            Nace = income.ClassficationOfEconomicActivityId,
            Profession = income.JobTypeId,
            Street = income.Address?.Street,
            HouseNumber = income.Address?.BuildingIdentificationNumber,
            StreetNumber = income.Address?.LandRegistryNumber,
            Postcode = getZipCode(income.Address?.Postcode),//TODO zmeni c4m long na string?
            City = income.Address?.City,
            CountryCode = (await _codebookService.Countries(_cancellationToken)).FirstOrDefault(t => t.Id == income.Address?.CountryId)?.ShortName,
            EstablishedOn = income.EstablishedOn,
            Domiciled = income.IsDomicile,
            ProofType = await getProofType<LoanApplicationEntrepreneurIncomeProofType>(income.ProofTypeId),
            AnnualIncomeAmount = income.AnnualIncomeAmount.ToAmount(),
            LumpSumModified = income.LumpSumModified,
            LumpSumTaxationRegime = income.LumpSumTaxationRegime
        };
    }

    private async Task<_C4M.LoanApplicationRentIncome?> mapIncomesRent(_V2.LoanApplicationRentIncome? income)
        => income is null ? null : new _C4M.LoanApplicationRentIncome
        {
            AccountNumber = income.BankAccount?.ConvertToString(),
            Domiciled = income.IsDomicile,
            MonthlyIncomeAmount = income.MonthlyIncomeAmount.ToAmount(),
            ProofType = await getProofType<LoanApplicationRentIncomeProofType>(income.ProofTypeId)
        };

    private async Task<List<_C4M.LoanApplicationOtherIncome>?> mapIncomesOther(List<_V2.LoanApplicationOtherIncome>? incomes)
        => incomes is null ? null : (await incomes.SelectAsync(async income => new _C4M.LoanApplicationOtherIncome
        {
            AccountNumber = income.BankAccount?.ConvertToString(),
            Domiciled = income.IsDomicile,
            MonthlyIncomeAmount = income.MonthlyIncomeAmount.ToAmount(),
            Type = (await _codebookService.IncomeOtherTypes(_cancellationToken)).FirstOrDefault(x => x.Id == income.IncomeOtherTypeId)?.Code,
            ProofType = await getProofType<LoanApplicationOtherIncomeProofType>(income.ProofTypeId)
        })).ToList();

    private static List<_C4M.IncomeDeduction> getDeductions(_V2.LoanApplicationEmploymentIncomeDeduction? deduction)
        => new List<IncomeDeduction>
        {
            new IncomeDeduction { Type = IncomeDeductionType.EXECUTION, Amount = (deduction?.Execution).ToAmountDefault() },
            new IncomeDeduction { Type = IncomeDeductionType.INSTALLMENTS, Amount = (deduction?.Installments).ToAmountDefault() },
            new IncomeDeduction { Type = IncomeDeductionType.OTHER, Amount = (deduction?.Other).ToAmountDefault() }
        };

    private async Task<TResponse?> getProofType<TResponse>(int? proofTypeId) where TResponse : struct
        => Helpers.GetEnumFromString<TResponse>((await _codebookService.ProofTypes(_cancellationToken)).FirstOrDefault(t => t.Id == proofTypeId)?.Code);

    private static long? getZipCode(string? zip)
    {
        long code;
        return long.TryParse(zip, out code) ? code : null;
    }

    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CancellationToken _cancellationToken;

    public HouseholdCustomerIncomeChildMapper(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        _codebookService = codebookService;
    }
}
