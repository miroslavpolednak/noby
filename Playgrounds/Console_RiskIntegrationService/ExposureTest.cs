using DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

namespace Console_RiskIntegrationService;

internal static class ExposureTest
{
    public static CustomersExposureCalculateRequest _test1 = new CustomersExposureCalculateRequest
    {
        SalesArrangementId = 149999990033,
        LoanApplicationDataVersion = "0005",
        RiskBusinessCaseId = "urn:ri:KBCZ.LAA.RiskBusinessCase.25628201"
    };
}
