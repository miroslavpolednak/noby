using ExternalServices.Rip.V1.RipWrapper;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
    public static GetLoanApplicationAssessmentResponse ToApiResponse(this LoanApplicationAssessmentResponse response)
    {
        return new GetLoanApplicationAssessmentResponse
        {
            Application = new()
            {
                LoanApplicationLimit = response.AssessmentDetail?.LoanApplicationLimit?._LoanApplicationLimit.Value,
                LoanApplicationInstallmentLimit = response.AssessmentDetail?.LoanApplicationLimit?.LoanApplicationInstallmentLimit?.Value,
                RemainingAnnuityLivingAmount = response.AssessmentDetail?.LoanApplicationLimit?.RemainingAnnuityLivingAmount?.Value,
                MonthlyIncomeAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInKBAmount.Value,
                MonthlyEntrepreneurInstallmentsInKBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyEntrepreneurInstallmentsInKBAmount.Value,
                MonthlyInstallmentsInMPSSAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = response.AssessmentDetail?.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInCBCBAmount.Value,
                CIR = response.AssessmentDetail?.LoanApplicationLimit?.Cir,
                DTI = response.AssessmentDetail?.LoanApplicationLimit?.Dti,
                DSTI = response.AssessmentDetail?.LoanApplicationLimit?.Dsti,
                LTC = response.CollateralRiskCharacteristics?.First().Ltp,
                LFTV = response.CollateralRiskCharacteristics?.First().Ltfv,
                LTV = response.CollateralRiskCharacteristics?.First().Ltv
            },
            Households = response.HouseholdAssessmentDetail?.Select(h => new Dto.Household
            {
                HouseholdId = h.HouseholdId,
                MonthlyIncomeAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.First().MonthlyIncomeAmount.Value,
                MonthlyCostsWithoutInstAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.First().MonthlyCostsWithoutInstAmount.Value,
                MonthlyInstallmentsInMPSSAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInMPSSAmount.Value,
                MonthlyInstallmentsInOFIAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInOFIAmount.Value,
                MonthlyInstallmentsInCBCBAmount = h.AssessmentDetail.LoanApplicationRiskCharacteristics?.First().MonthlyInstallmentsInCBCBAmount.Value,
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
            }).ToList()
        };
    }
}
