using NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;
using cRS = DomainServices.RiskIntegrationService.Contracts.Shared;
using cOffer = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2;

internal static class Extensions
{
    const string _creditorNameKB = "KB";
    const string _creditorNameJPU = "JPÚ";

    public static bool BankAccountEquals(this cRS.BankAccountDetail account1, cRS.BankAccountDetail? account2)
    {
        return (account1.NumberPrefix?.Equals(account2?.NumberPrefix, StringComparison.OrdinalIgnoreCase) ?? false)
            && (account1.Number?.Equals(account2?.Number, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    public static Dto.HouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureRequestedKBGroupItem item,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        return CreateHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            _creditorNameKB,
            item.LoanTypeCategory,
            item.InstallmentAmount,
            isEntrepreneur);
    }

    public static Dto.HouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureRequestedCBCBItem item,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
            var result = CreateHouseholdItem(
                2,
                obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
                _creditorNameJPU,
                item.LoanType,
                item.InstallmentAmount,
                isEntrepreneur);
            result.KbGroupInstanceCode = item.KbGroupInstanceCode;
            result.CbcbDataLastUpdate = item.CbcbDataLastUpdate;
            return result;
    }

    public static Dto.HouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureExistingKBGroupItem item,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
            var result = CreateHouseholdItem(
                2,
                obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
                _creditorNameKB,
                item.LoanType, 
                item.InstallmentAmount,
                isEntrepreneur,
                item.ExposureAmount,
                item.ContractDate,
                item.MaturityDate);
            result.DrawingAmount = item.DrawingAmount;
            if (item.BankAccount is not null) {
                result.BankAccount = new NOBY.Dto.BankAccount
                {
                    AccountPrefix = item.BankAccount.NumberPrefix,
                    AccountNumber = item.BankAccount.Number,
                    AccountBankCode = item.BankAccount.BankCode
                };
            }
            return result;
    }

    public static List<Dto.HouseholdObligationItem> CreateHouseholdObligations(
        this List<DomainServices.HouseholdService.Contracts.Obligation> items,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes)
    {
        return items.Select(t =>
            {
                var obligationType = obligationTypes.First(x => x.Id == t.ObligationTypeId);
                bool isLimit = obligationType?.ObligationProperty.Equals("limit", StringComparison.OrdinalIgnoreCase) ?? false;

                return new Dto.HouseholdObligationItem
                {
                    ObligationTypeOrder = obligationType?.Order ?? 0,
                    ObligationSourceId = 1,
                    ObligationTypeId = obligationType?.Id ?? 0,
                    ObligationTypeName = obligationType?.Name ?? "Neznázmý",
                    LoanPrincipalAmount = isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(t.LoanPrincipalAmount, t.Correction?.LoanPrincipalAmountCorrection),
                    InstallmentAmount = isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(t.InstallmentAmount, t.Correction?.InstallmentAmountCorrection),
                    CreditCardLimit = !isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(t.CreditCardLimit, t.Correction?.CreditCardLimitCorrection),
                    CreditorName = t.Creditor?.IsExternal ?? false ? _creditorNameJPU : _creditorNameKB,
                };
            })
            .ToList();
    }

    public static Dto.HouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureExistingCBCBItem item,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        var result = CreateHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            _creditorNameJPU,
            item.LoanType,
            item.InstallmentAmount,
            isEntrepreneur,
            item.ExposureAmount,
            item.ContractDate,
            item.MaturityDate);
        result.KbGroupInstanceCode = item.KbGroupInstanceCode;
        result.CbcbDataLastUpdate = item.CbcbDataLastUpdate;
        return result;
    }

    private static Dto.HouseholdObligationItem CreateHouseholdItem(
        in int sourceId,
        ObligationTypesResponse.Types.ObligationTypeItem? obligationType,
        in string creditorName,
        in int? loanType,
        in decimal? installmentAmount,
        in bool isEntrepreneur,
        in decimal? exposureAmount = null,
        in DateTime? contractDate = null,
        in DateTime? maturityDate = null)
    {
        bool isLimit = obligationType?.ObligationProperty.Equals("limit", StringComparison.OrdinalIgnoreCase) ?? false;
        
        return new Dto.HouseholdObligationItem
        {
            ObligationTypeOrder = obligationType?.Order ?? 0,
            ObligationSourceId = sourceId,
            IsEntrepreneur = isEntrepreneur,
            ObligationTypeId = obligationType?.Id ?? 0,
            ObligationTypeName = obligationType?.Name ?? "Neznázmý",
            LoanPrincipalAmount = isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(exposureAmount),
            InstallmentAmount = isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(installmentAmount),
            CreditCardLimit = !isLimit ? null : Dto.HouseholdObligationItem.Amount.Create(exposureAmount),
            ObligationLaExposureId = loanType.GetValueOrDefault(),
            CreditorName = creditorName,
            ContractDate = contractDate,
            MaturityDate = maturityDate
        };
    }

    public static bool GetDisplayAssessmentResultInfoTextToReasonsApiResponse(
        this cRS.V1.LoanApplicationAssessmentResponse response, 
        List<DomainServices.HouseholdService.Contracts.CustomerOnSA> customers,
        Dictionary<int, List<DomainServices.HouseholdService.Contracts.Obligation>> obligations)
    {
        if (response.AssessmentResult == 502 && (response.Reasons?.Any(t => t.Code == "060009") ?? false))
        {
            foreach (var customer in customers)
            {
                if (obligations.TryGetValue(customer.CustomerOnSAId, out List<DomainServices.HouseholdService.Contracts.Obligation>? customerObligations)
                    && customerObligations.Any(t => ((t.Creditor is not null && !t.Creditor.IsExternal.GetValueOrDefault()) && (t.Correction is not null && t.Correction.CorrectionTypeId.GetValueOrDefault() != 1))))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static List<AssessmentReason>? ToReasonsApiResponse(this cRS.V1.LoanApplicationAssessmentResponse response)
        => response
            .Reasons?
            .Select(r => new AssessmentReason
            {
                Code = r.Code,
                Description = r.Description,
                Level = r.Level,
                Result = r.Result,
                Target = r.Target,
                Weight = r.Weight,
            })
            .ToList();

    public static LoanApplication ToLoanApplicationApiResponse(this cRS.V1.LoanApplicationAssessmentResponse response, cOffer.GetMortgageOfferResponse? offer)
        => new LoanApplication
        {
            Limit = response.Detail?.Limit?.Limit?.Amount,
            InstallmentLimit = response.Detail?.Limit?.InstallmentLimit?.Amount,
            RemainingAnnuityLivingAmount = response.Detail?.Limit?.RemainingAnnuityLivingAmount?.Amount,
            MonthlyIncome = response.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
            MonthlyCostsWithoutInstallments = response.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
            MonthlyInstallmentsInKB = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
            MonthlyEntrepreneurInstallmentsInKB = response.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
            MonthlyInstallmentsInMPSS = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
            MonthlyInstallmentsInOFI = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
            MonthlyInstallmentsInCBCB = response.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
            CIR = response.Detail?.Limit?.Cir,
            DTI = response.Detail?.Limit?.Dti,
            DSTI = response.Detail?.Limit?.Dsti,
            LTCP = response.CollateralRiskCharacteristics?.Ltp,
            LFTV = response.CollateralRiskCharacteristics?.Lftv,
            LTV = response.CollateralRiskCharacteristics?.Ltv,
            LoanAmount = offer?.SimulationResults?.LoanAmount!,
            LoanPaymentAmount = offer?.SimulationResults?.LoanPaymentAmount,
        };

    public static Dto.HouseholdRisk ToHouseholdRiskApiResponse(this cRS.V1.LoanApplicationAssessmentHouseholdDetail household)
        => new Dto.HouseholdRisk
        {
            MonthlyIncome = household.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
            MonthlyCostsWithoutInstallments = household.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
            MonthlyInstallmentsInMPSS = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
            MonthlyInstallmentsInOFI = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
            MonthlyInstallmentsInCBCB = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
            MonthlyInstallmentsInKBAmount = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
            MonthlyEntrepreneurInstallmentsInKBAmount = household.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
            CIR = household.Detail?.Limit?.Cir,
            DTI = household.Detail?.Limit?.Dti,
            DSTI = household.Detail?.Limit?.Dsti,
            LoanApplicationLimit = household.Detail?.Limit?.Limit?.Amount,
            LoanApplicationInstallmentLimit = household.Detail?.Limit?.InstallmentLimit?.Amount,
        };
}
