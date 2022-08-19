namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;

internal partial class ResourceIdentifier
{
    public static ResourceIdentifier CreateLoanApplication(long id, string variant)
        => new ResourceIdentifier
        {
            Id = $"{id}",
            Variant = variant,
            Instance = Constants.MPSS,
            Domain = Constants.LA,
            Resource = Constants.LoanApplication
        };

    public static ResourceIdentifier? CreateResourceProcess(string? id, string variant)
        => string.IsNullOrEmpty(id) ? null : new ResourceIdentifier
        {
            Id = id,
            Variant = variant,
            Domain = Constants.OM,
            Instance = Constants.MPSS,
            Resource = Constants.OfferInstance
        };

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
