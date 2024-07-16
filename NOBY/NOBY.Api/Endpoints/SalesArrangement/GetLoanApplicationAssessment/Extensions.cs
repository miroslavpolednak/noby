using cRS = DomainServices.RiskIntegrationService.Contracts.Shared;
using cOffer = DomainServices.OfferService.Contracts;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
    public static bool BankAccountEquals(this cRS.BankAccountDetail account1, cRS.BankAccountDetail? account2)
    {
        return (account1.NumberPrefix?.Equals(account2?.NumberPrefix, StringComparison.OrdinalIgnoreCase) ?? false)
            && (account1.Number?.Equals(account2?.Number, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    public static SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureRequestedKBGroupItem item,
        List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> laExposureItems,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        return createHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            laExposureItems.FirstOrDefault(t => t.Id == item.LoanType),
            item.CustomerRoleId,
            false,
            item.InstallmentAmount,
            isEntrepreneur,
            null,
            item.LoanAmount);
    }

    public static SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureRequestedCBCBItem item,
        List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> laExposureItems,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        var result = createHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            laExposureItems.FirstOrDefault(t => t.Id == item.LoanType),
            item.CustomerRoleId,
            true,
            //item.LoanType,
            item.InstallmentAmount,
            isEntrepreneur,
            null,
            item.LoanAmount);
        result.KbGroupInstanceCode = item.KbGroupInstanceCode;
        result.CbcbDataLastUpdate = item.CbcbDataLastUpdate;
        return result;
    }

    public static SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureExistingKBGroupItem item,
        List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> laExposureItems,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        var result = createHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            laExposureItems.FirstOrDefault(t => t.Id == item.LoanType),
            item.CustomerRoleId,
            false,
            item.InstallmentAmount,
            isEntrepreneur,
            item.ExposureAmount,
            item.LoanAmount,
            item.ContractDate,
            item.MaturityDate);
        result.DrawingAmount = item.DrawingAmount;
        if (item.BankAccount is not null)
        {
            result.BankAccount = new()
            {
                AccountPrefix = item.BankAccount.NumberPrefix,
                AccountNumber = item.BankAccount.Number,
                AccountBankCode = item.BankAccount.BankCode
            };
        }
        return result;
    }

    public static List<SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem> CreateHouseholdObligations(
        this List<DomainServices.HouseholdService.Contracts.Obligation> items,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes)
    {
        return items.Select(t =>
            {
                var obligationType = obligationTypes.First(x => x.Id == t.ObligationTypeId);
                bool isLimit = obligationType?.ObligationProperty.Equals("limit", StringComparison.OrdinalIgnoreCase) ?? false;

                return new SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem
                {
                    ObligationTypeOrder = obligationType?.Order ?? 0,
                    ObligationSourceId = 1,
                    ObligationTypeId = obligationType?.Id ?? 0,
                    ObligationTypeName = obligationType?.Name ?? "Neznázmý",
                    AmountConsolidated = t.AmountConsolidated,
                    LoanAmountCurrent = isLimit ? null : t.LoanPrincipalAmount,
                    CreditCardLimit = !isLimit ? null : t.CreditCardLimit,
                    InstallmentAmount = isLimit ? null : t.InstallmentAmount,
                    Creditor = t.Creditor is null ? null : new()
                    {
                        CreditorId = t.Creditor.CreditorId,
                        IsExternal = t.Creditor.IsExternal,
                        Name = t.Creditor.Name
                    },
                    Correction = t.Correction is null ? null : new SharedTypesCustomerObligationCorrection
                    {
                        CorrectionTypeId = t.Correction.CorrectionTypeId,
                        LoanPrincipalAmountCorrection = t.Correction.LoanPrincipalAmountCorrection,
                        CreditCardLimitCorrection = t.Correction.CreditCardLimitCorrection,
                        InstallmentAmountCorrection = t.Correction.InstallmentAmountCorrection
                    }
                };
            })
            .ToList();
    }

    public static SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem CreateHouseholdObligations(
        this CustomerExposureExistingCBCBItem item,
        List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> laExposureItems,
        List<ObligationTypesResponse.Types.ObligationTypeItem> obligationTypes,
        bool isEntrepreneur)
    {
        var result = createHouseholdItem(
            2,
            obligationTypes.FirstOrDefault(t => t.Id == item.LoanTypeCategory.GetValueOrDefault()),
            laExposureItems.FirstOrDefault(t => t.Id == item.LoanType),
            item.CustomerRoleId,
            true,
            item.InstallmentAmount,
            isEntrepreneur,
            item.ExposureAmount,
            item.LoanAmount,
            item.ContractDate,
            item.MaturityDate);
        result.KbGroupInstanceCode = item.KbGroupInstanceCode;
        result.CbcbDataLastUpdate = item.CbcbDataLastUpdate;
        return result;
    }

    private static SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem createHouseholdItem(
        in int sourceId,
        ObligationTypesResponse.Types.ObligationTypeItem? obligationType,
        ObligationLaExposuresResponse.Types.ObligationLaExposureItem? loanType,
        in int? roleId,
        in bool isExternal,
        in decimal? installmentAmount,
        in bool isEntrepreneur,
        in decimal? exposureAmount = null,
        in decimal? loanAmount = null,
        in DateTime? contractDate = null,
        in DateTime? maturityDate = null)
    {
        bool isLimit = obligationType?.ObligationProperty.Equals("limit", StringComparison.OrdinalIgnoreCase) ?? false;

        return new SalesArrangementGetLoanApplicationAssessmentHouseholdObligationItem
        {
            ObligationTypeOrder = obligationType?.Order ?? 0,
            ObligationSourceId = sourceId,
            IsEntrepreneur = isEntrepreneur,
            ObligationTypeId = obligationType?.Id ?? 0,
            ObligationTypeName = obligationType?.Name ?? "Neznázmý",
            LoanAmountCurrent = isLimit ? null : exposureAmount,
            CreditCardLimit = !isLimit ? null : loanAmount,
            LoanAmountTotal = isLimit ? null : loanAmount,
            InstallmentAmount = isLimit ? null : installmentAmount,
            ObligationLaExposureId = loanType?.Id,
            ObligationLaExposureName = loanType?.Name,
            RoleId = roleId,
            Creditor = new()
            {
                IsExternal = isExternal
            },
            ContractDate = contractDate.HasValue ? DateOnly.FromDateTime(contractDate.Value) : null,
            MaturityDate = maturityDate.HasValue ? DateOnly.FromDateTime(maturityDate.Value) : null,
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
                    && customerObligations.Any(t => t.Creditor is not null && !t.Creditor.IsExternal.GetValueOrDefault() && t.Correction is not null && t.Correction.CorrectionTypeId.GetValueOrDefault() != 1))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static List<SalesArrangementGetLoanApplicationAssessmentAssessmentReason>? ToReasonsApiResponse(this cRS.V1.LoanApplicationAssessmentResponse response)
        => response
            .Reasons?
            .Select(r => new SalesArrangementGetLoanApplicationAssessmentAssessmentReason
            {
                Code = r.Code,
                Description = r.Description,
                Level = r.Level,
                Result = r.Result,
                Target = r.Target,
                Weight = r.Weight,
            })
            .ToList();

    public static SalesArrangementGetLoanApplicationAssessmentLoanApplication ToLoanApplicationApiResponse(this cRS.V1.LoanApplicationAssessmentResponse response, cOffer.GetOfferResponse? offer)
        => new()
        {
            Score = string.IsNullOrEmpty(response.Detail?.Score?.Scale) ? null : $"{response.Detail.Score.Scale} {response.Detail.Score.Value}",
            CollateralLimit = response.Detail?.Limit?.CollateralLimit?.Amount,
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
            Cir = response.Detail?.Limit?.Cir,
            Dti = response.Detail?.Limit?.Dti,
            Dsti = response.Detail?.Limit?.Dsti,
            Ltcp = response.CollateralRiskCharacteristics?.Ltp,
            Lftv = response.CollateralRiskCharacteristics?.Lftv,
            Ltv = response.CollateralRiskCharacteristics?.Ltv,
            LoanAmount = offer?.MortgageOffer.SimulationResults?.LoanAmount!,
            LoanPaymentAmount = offer?.MortgageOffer.SimulationResults?.LoanPaymentAmount,
        };

    public static SalesArrangementGetLoanApplicationAssessmentHouseholdRisk ToHouseholdRiskApiResponse(this cRS.V1.LoanApplicationAssessmentHouseholdDetail household)
        => new()
        {
            MonthlyIncome = household.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
            MonthlyCostsWithoutInstallments = household.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
            MonthlyInstallmentsInMPSS = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
            MonthlyInstallmentsInOFI = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
            MonthlyInstallmentsInCBCB = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
            MonthlyInstallmentsInKBAmount = household.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
            MonthlyEntrepreneurInstallmentsInKBAmount = household.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
            Cir = household.Detail?.Limit?.Cir,
            Dti = household.Detail?.Limit?.Dti,
            Dsti = household.Detail?.Limit?.Dsti,
            LoanApplicationLimit = household.Detail?.Limit?.Limit?.Amount,
            LoanApplicationInstallmentLimit = household.Detail?.Limit?.InstallmentLimit?.Amount,
        };
}
