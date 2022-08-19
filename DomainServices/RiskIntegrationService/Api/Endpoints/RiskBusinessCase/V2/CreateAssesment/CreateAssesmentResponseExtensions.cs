using DomainServices.RiskIntegrationService.Contracts.Shared;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;
using _sh = DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssesment;

internal static class CreateAssesmentExtensions
{
    public static _sh.LoanApplicationAssessmentResponse ToRIP(this _C4M.Identified response)
        => new ()
        {
            LoanApplicationAssessmentId = response.Id,
            SalesArrangementId = response.LoanApplicationId?.GetSalesArrangementId(),
            RiskBusinesscaseId = response.RiskBusinesscaseId?.Id,
            RiskBusinessCaseExpirationDate = response.RiskBusinesscaseExpirationDate?.DateTime,
            AssessmentResult = response.AssessmentResult,
            StandardRiskCosts = response.StandardRiskCosts,
            GlTableCode = response.GlTableCode,
            Reasons = response.LoanApplicationAssessmentReason?.Select(Reason()).ToList(),
            Detail = response.AssessmentDetail != null ? response.AssessmentDetail.ToDetail() : null,
            HouseholdsDetails = response.HouseholdAssessmentDetail?.Select(Household()).ToList(),
            CustomersDetails = response.CounterpartyAssessmentDetail?.Select(Customer()).ToList(),
            CollateralRiskCharacteristics = response.CollateralRiskCharacteristics?.ToCollateral(),
            Version = response.Version?.ToVersion(),
            Created = response.Created?.ToChangeDetail(),
            Updated = response.Updated?.ToChangeDetail()
        };

    private static _sh.LoanApplicationAssessmentDetail ToDetail(this _C4M.AssessmentDetail detail)
        => new()
        {
            Score = detail.LoanApplicationScore != null ?
            new _sh.LoanApplicationAssessmentScore
            {
                Scale = detail.LoanApplicationScore.Scale,
                Value = detail.LoanApplicationScore.Value
            } : null,
            Limit = detail.LoanApplicationLimit != null ?
            new _sh.LoanApplicationAssesmentLimit
            {
                Limit = detail.LoanApplicationLimit.LoanApplicationLimit1.ToAmountDetail(),
                InstallmentLimit = detail.LoanApplicationLimit.LoanApplicationInstallmentLimit.ToAmountDetail(),
                CollateralLimit = detail.LoanApplicationLimit.LoanApplicationCollateralLimit.ToAmountDetail(),
                RemainingAnnuityLivingAmount = detail.LoanApplicationLimit.RemainingAnnuityLivingAmount.ToAmountDetail(),
                IsCalculationStressed = detail.LoanApplicationLimit.CalculationIrStressed.GetValueOrDefault()
            } : null
        };

    private static Func<_C4M.LoanApplicationAssessmentReason, _sh.LoanApplicationAssessmentReason> Reason()
        => t => new ()
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
        => t => new ()
        {
            HouseholdId = t.HouseholdId,
            Detail = t.AssessmentDetail.ToDetail()
        };

    private static Func<_C4M.CounterpartyAssessmentDetail, _sh.LoanApplicationAssessmentCustomerDetail> Customer()
        => t => new ()
        {
            InternalCustomerId = t.CounterPartyId,
            PrimaryCustomerId = t.CustomerId?.GetPrimaryCustomerId(),
            AssessmentDetail = t.AssessmentDetail.ToDetail()
        };

    private static _sh.LoanApplicationAssessmentCollateralRiskCharacteristics ToCollateral(this _C4M.CollateralRiskCharacteristics collateral)
        => new()
        {
            Ltv = collateral.Ltv,
            Ltfv = collateral.Ltfv,
            Ltp = collateral.Ltp,
            SumAppraisedValue = collateral.SumAppraisedValue
        };

    private static string ToVersion(this _C4M.SemanticVersion version)
        => string.IsNullOrEmpty(version.NonSemanticPart) ?
        $"{version.Major}.{version.Minor}.{version.Bugfix}" :
        $"{version.Major}.{version.Minor}.{version.Bugfix}.{version.NonSemanticPart}";

    private static ChangeDetail ToChangeDetail(this _C4M.Change change)
        => new()
        {
            IdentityId = change.IdentityId,
            ChangeTime = change.Timestamp?.DateTime
        };
}
