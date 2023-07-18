using NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;
using cRS = DomainServices.RiskIntegrationService.Contracts.Shared;
using cOffer = DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

internal static class Extensions
{
    public static GetLoanApplicationAssessmentResponse ToApiResponse(this cRS.V1.LoanApplicationAssessmentResponse response, cOffer.GetMortgageOfferDetailResponse? offer)
    {
        //https://wiki.kb.cz/pages/viewpage.action?pageId=464683017

        return new GetLoanApplicationAssessmentResponse
        {
            Application = new LoanApplication
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
            },
            Households = response.HouseholdsDetails?.Select(h => new Dto.Household
            {
                HouseholdId = h.HouseholdId,
                MonthlyIncome = h.Detail?.RiskCharacteristics?.MonthlyIncome?.Amount,
                MonthlyCostsWithoutInstallments = h.Detail?.RiskCharacteristics?.MonthlyCostsWithoutInstallments?.Amount,
                MonthlyInstallmentsInMPSS = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInMPSS?.Amount,
                MonthlyInstallmentsInOFI = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInOFI?.Amount,
                MonthlyInstallmentsInCBCB = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInCBCB?.Amount,
                MonthlyInstallmentsInKBAmount = h.Detail?.RiskCharacteristics?.MonthlyInstallmentsInKB?.Amount,
                MonthlyEntrepreneurInstallmentsInKBAmount = h.Detail?.RiskCharacteristics?.MonthlyEntrepreneurInstallmentsInKB?.Amount,
                CIR = h.Detail?.Limit?.Cir,
                DTI = h.Detail?.Limit?.Dti,
                DSTI = h.Detail?.Limit?.Dsti,
                LoanApplicationLimit = h.Detail?.Limit?.Limit?.Amount,
                LoanApplicationInstallmentLimit = h.Detail?.Limit?.InstallmentLimit?.Amount,
            }).ToList(),
            RiskBusinesscaseExpirationDate = response!.RiskBusinessCaseExpirationDate,
            AssessmentResult = response.AssessmentResult,
            Reasons = response.Reasons?.Select(r => new AssessmentReason
            {
                Code = r.Code,
                Description = r.Description,
                Level = r.Level,
                Result = r.Result,
                Target = r.Target,
                Weight = r.Weight,
            }).ToList()
        };
    }
}
