using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

namespace Console_RiskIntegrationService;

internal static class ExposureTest
{
    public static CustomerExposureCalculateRequest _test1 = new CustomerExposureCalculateRequest
    {
        SalesArrangementId = 149999990033,
        LoanApplicationDataVersion = "0005",
        RiskBusinessCaseId = "urn:ri:KBCZ.LAA.RiskBusinessCase.25628201"
    };
}
