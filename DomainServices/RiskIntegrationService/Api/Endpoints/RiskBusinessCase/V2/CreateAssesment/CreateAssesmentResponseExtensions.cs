using DomainServices.RiskIntegrationService.Contracts.Shared;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesment;

internal static class CreateAssesmentExtensions
{
    public static _sh.LoanApplicationAssessmentResponse ToRIP(this _C4M.Identified response)
        => new()
        {
            LoanApplicationAssessmentId = response.Id,
            SalesArrangementId = response.LoanApplicationId.ToSalesArrangementId(),
            RiskBusinesscaseId = response.RiskBusinesscaseId,
            RiskBusinessCaseExpirationDate = response.RiskBusinesscaseExpirationDate,
            AssessmentResult = response.AssessmentResult,
            StandardRiskCosts = response.StandardRiskCosts,
            GlTableCode = response.GlTableCode,
            Reasons = response.LoanApplicationAssessmentReason?.Select(Reason()).ToList(),
            Detail = response.AssessmentDetail?.ToDetail(),
            HouseholdsDetails = response.HouseholdAssessmentDetail?.Select(Household()).ToList(),
            CustomersDetails = response.CounterpartyAssessmentDetail?.Select(Customer()).ToList(),
            CollateralRiskCharacteristics = response.CollateralRiskCharacteristics?.ToCollateral(),
            ApprovalPossibility = response.LoanApplicationApprovalPossibilities?.ToApprovalPossibility(),
            Version = response.Version?.ToVersion(),
            Created = response.Created?.ToChangeDetail(),
            Updated = response.Updated?.ToChangeDetail()
        };

    private static _sh.LoanApplicationAssessmentApprovalPossibility ToApprovalPossibility(this _C4M.LoanApplicationApprovalPossibilities model)
        => new()
        {
            SelfApprovalPossible = model.SelfApprovalPossible,
            AutoApprovalPossible = model.AutoApprovalPossible
        };

    private static _sh.LoanApplicationAssessmentDetail ToDetail(this _C4M.AssessmentDetail model)
        => new()
        {
            Score = model.LoanApplicationScore?.ToScore(),
            Limit = model.LoanApplicationLimit?.ToLimit(),
            RiskCharacteristics = model.LoanApplicationRiskCharacteristics?.ToRiskCharacteristics()
        };

    private static _sh.LoanApplicationAssessmentScore ToScore(this _C4M.LoanApplicationScore model)
        => new()
        {
            Scale = model.Scale,
            Value = model.Value
        };

    private static _sh.LoanApplicationAssesmentLimit ToLimit(this _C4M.LoanApplicationLimit model)
        => new()
        {
            Limit = model.LoanApplicationLimit1.ToAmountDetail(),
            InstallmentLimit = model.LoanApplicationInstallmentLimit.ToAmountDetail(),
            CollateralLimit = model.LoanApplicationCollateralLimit.ToAmountDetail(),
            RemainingAnnuityLivingAmount = model.RemainingAnnuityLivingAmount.ToAmountDetail(),
            IsCalculationStressed = model.CalculationIrStressed.GetValueOrDefault(),
            Iir = model.Iir,
            Cir = model.Cir,
            Dti = model.Dti,
            Dsti = model.Dsti
        };

    private static _sh.LoanApplicationAssesmentRiskCharacteristics ToRiskCharacteristics(this _C4M.RiskCharacteristics model)
        => new()
        {
            MonthlyIncome = model.MonthlyIncomeAmount.ToAmountDetail(),
            MonthlyCostsWithoutInstallments = model.MonthlyCostsWithoutInstAmount.ToAmountDetail(),
            MonthlyInstallmentsInKB = model.MonthlyInstallmentsInKBAmount.ToAmountDetail(),
            MonthlyEntrepreneurInstallmentsInKB = model.MonthlyEntrepreneurInstallmentsInKBAmount.ToAmountDetail(),
            MonthlyInstallmentsInMPSS = model.MonthlyInstallmentsInMPSSAmount.ToAmountDetail(),
            MonthlyInstallmentsInOFI = model.MonthlyInstallmentsInOFIAmount.ToAmountDetail(),
            MonthlyInstallmentsInCBCB = model.MonthlyInstallmentsInCBCBAmount.ToAmountDetail(),
        };

    private static Func<_C4M.LoanApplicationAssessmentReason, _sh.LoanApplicationAssessmentReason> Reason()
        => t => new()
        {
            Code = t.Code,
            Level = t.Level,
            Weight = t.Weight,
            Category = t.Category,
            Target = t?.Detail.Target,
            Description = t?.Detail.Desc,
            Result = t?.Detail.Result
        };

    private static Func<_C4M.HouseholdAssessmentDetail, _sh.LoanApplicationAssessmentHouseholdDetail> Household()
        => t => new()
        {
            HouseholdId = t.HouseholdId,
            Detail = t.AssessmentDetail.ToDetail()
        };

    private static Func<_C4M.CounterpartyAssessmentDetail, _sh.LoanApplicationAssessmentCustomerDetail> Customer()
        => t => new()
        {
            InternalCustomerId = t.CounterPartyId,
            //TODO C4M 
            PrimaryCustomerId = t.CustomerId?.ToPrimaryCustomerId(),
            AssessmentDetail = t.AssessmentDetail.ToDetail()
        };

    private static _sh.LoanApplicationAssessmentCollateralRiskCharacteristics ToCollateral(this _C4M.CollateralRiskCharacteristics model)
        => new()
        {
            Ltv = model.Ltv,
            Ltfv = model.Ltfv,
            Ltp = model.Ltp,
            SumAppraisedValue = model.SumAppraisedValue,
            TotalUsedValue = model.TotalCollUsedValue
        };

    private static string ToVersion(this _C4M.SemanticVersion model)
        => string.IsNullOrEmpty(model.NonSemanticPart) ?
        $"{model.Major}.{model.Minor}.{model.Bugfix}" :
        $"{model.Major}.{model.Minor}.{model.Bugfix}.{model.NonSemanticPart}";

    private static ChangeDetail ToChangeDetail(this _C4M.Change model)
        => new()
        {
            IdentityId = model.IdentityId,
            ChangeTime = model.Timestamp
        };

    private static AmountDetail? ToAmountDetail(this _C4M.Amount model)
        => model != null && model.Value != null ? new()
        {
            Amount = model.Value.Value,
            CurrencyCode = model.CurrencyCode
        } : null;
}
