using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
    public static GetLoanApplicationAssessmentResponse ToApiResponse(this LoanApplicationAssessmentResponse response)
    {
        return new GetLoanApplicationAssessmentResponse
        {
            /*Application = new()
            {
                LoanApplicationLimit = response.AssessmentDetail?.LoanApplicationLimit?._LoanApplicationLimit.Value,
                LoanApplicationInstallmentLimit = response.AssessmentDetail?.LoanApplicationLimit?.LoanApplicationInstallmentLimit?.Value,
                RemainingAnnuityLivingAmount = response.AssessmentDetail?.LoanApplicationLimit?.RemainingAnnuityLivingAmount?.Value,
                MonthlyIncomeAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInKBAmount.Value,
                MonthlyEntrepreneurInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKBAmount.Value,
                MonthlyInstallmentsInMPSSAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInCBCBAmount.Value,
                CIR = response.AssessmentDetail?.LoanApplicationLimit?.Cir,
                DTI = response.AssessmentDetail?.LoanApplicationLimit?.Dti,
                DSTI = response.AssessmentDetail?.LoanApplicationLimit?.Dsti,
                LTC = response.CollateralRiskCharacteristics?.Ltp,
                LFTV = response.CollateralRiskCharacteristics?.Ltfv,
                LTV = response.CollateralRiskCharacteristics?.Ltv
            },
            Households = response.HouseholdAssessmentDetail?.Select(h => new Dto.Household
            {
                HouseholdId = h.HouseholdId,
                MonthlyIncomeAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInMPSSAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.MonthlyInstallmentsInCBCBAmount.Value,
                CIR = h.AssessmentDetail?.LoanApplicationLimit?.Cir,
                DTI = h.AssessmentDetail?.LoanApplicationLimit?.Dti,
                DSTI = h.AssessmentDetail?.LoanApplicationLimit?.Dsti
            }).ToList(),
            RiskBusinesscaseExpirationDate = response.RiskBusinesscaseExpirationDate,
            AssessmentResult = response.AssessmentResult,
            Reasons = response.LoanApplicationAssessmentReason?.Select(r => new Dto.AssessmentReason
            {
                Code = r.Code,
                Desc = r.Detail?.Desc,
                Level = r.Level,
                Result = r.Detail?.Result,
                Target = r.Detail?.Target,
                Weight = r.Weight
            }).ToList()*/
        };
    }
}
