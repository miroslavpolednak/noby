using DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using CIS.Core;
using System.Threading;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class HouseholdCustomerIncomeChildMapper
{
    public async Task<_C4M.LoanApplicationIncome> MapIncomes(_V2.LoanApplicationIncome? income, bool verification)
    {
        if (income is null) return new LoanApplicationIncome { IncomeCollected = false };

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
                EmployerType = Helpers.GetEnumFromInt<LoanApplicationEmploymentIncomeEmployerType>(t.WorkSectorId),
                EmployerName = t.EmployerName,
                Nace = t.ClassficationOfEconomicActivityId,
                Profession = Helpers.GetEnumFromInt<LoanApplicationEmploymentIncomeProfession>(t.JobTypeId, LoanApplicationEmploymentIncomeProfession._13),
                Street = t.Address?.Street,
                //HouseNumber = t.Address?.BuildingIdentificationNumber,//TODO c4m predela na string
                //StreetNumber = t.Address?.LandRegistryNumber,
                //Postcode = t.Address?.Postcode,
                City = t.Address?.City,
                CountryCode = (await _codebookService.Countries(_cancellationToken)).FirstOrDefault(x => x.Id == t.Address?.CountryId)?.ShortName,
                JobTitle = t.JobDescription,
                Phone = string.IsNullOrEmpty(t.PhoneNumber) ? null : new PhoneContact { ContactType = PhoneContactContactType.BUSINESS, PhoneNumber = t.PhoneNumber },
                AccountNumber = t.BankAccount?.ConvertToString(),
                Domiciled = t.IsDomicile,
                ProofType = await getProofType<LoanApplicationEmploymentIncomeProofType>(t.ProofTypeId),
                DeclaredMonthIncome = Convert.ToDouble(t.MonthlyAmount),
                ForeignEmploymentType = (await _codebookService.IncomeForeignTypes(_cancellationToken)).FirstOrDefault(x => x.Id == t.IncomeForeignTypeId)?.Code,
                GrossAnnualIncome = t.GrossAnnualIncome.HasValue ? Convert.ToDouble(t.GrossAnnualIncome) : null,
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
            Profession = Helpers.GetEnumFromInt<LoanApplicationEntrepreneurIncomeProfession>(income.JobTypeId),
            Street = income.Address?.Street,
            //HouseNumber = income.Address?.BuildingIdentificationNumber,
            //StreetNumber = income.Address?.LandRegistryNumber,//TODO zmeni c4m long na string?
            //Postcode = income.Address?.Postcode,
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
            MonthlyIncomeAmount = income.MonthlyAmount.ToAmount(),
            ProofType = await getProofType<LoanApplicationRentIncomeProofType>(income.ProofTypeId)
        };

    private async Task<List<_C4M.LoanApplicationOtherIncome>?> mapIncomesOther(List<_V2.LoanApplicationOtherIncome>? incomes)
        => incomes is null ? null : (await incomes.SelectAsync(async income => new _C4M.LoanApplicationOtherIncome
        {
            AccountNumber = income.BankAccount?.ConvertToString(),
            Domiciled = income.IsDomicile,
            MonthlyIncomeAmount = income.MonthlyAmount.ToAmount(),
            Type = (await _codebookService.IncomeOtherTypes(_cancellationToken)).FirstOrDefault(x => x.Id == income.IncomeOtherTypeId)?.Code,
            ProofType = await getProofType<LoanApplicationOtherIncomeProofType>(income.ProofTypeId)
        })).ToList();

    private static List<_C4M.IncomeDeduction> getDeductions(_V2.LoanApplicationEmploymentIncomeDeduction? deduction)
        => new List<IncomeDeduction>
        {
            new IncomeDeduction { Type = IncomeDeductionType.EXECUTION, Amount = deduction?.Execution.ToAmount() },
            new IncomeDeduction { Type = IncomeDeductionType.INSTALLMENTS, Amount = deduction?.Installments.ToAmount() },
            new IncomeDeduction { Type = IncomeDeductionType.OTHER, Amount = deduction?.Other.ToAmount() },
        };

    private async Task<TResponse?> getProofType<TResponse>(int? proofTypeId) where TResponse : struct
        => Helpers.GetEnumFromString<TResponse>((await _codebookService.ProofTypes(_cancellationToken)).FirstOrDefault(t => t.Id == proofTypeId)?.Code);

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
