namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplicationAssessment.V0_2.Contracts;

internal partial class ResourceIdentifier
{
    public long GetSalesArrangementId()
    {
        var s = Id.Split(".");
        return long.Parse(s[s.Length - 1].Split("~")[0], System.Globalization.CultureInfo.InvariantCulture);
    }

    public string GetPrimaryCustomerId()
    {
        var s = Id.Split(".");
        return s[s.Length - 1];
    }
}
