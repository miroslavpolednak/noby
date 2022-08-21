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

    public static ResourceIdentifier? CreateLoanSoldProduct(string id, string? instance, string variant)
        => new ResourceIdentifier
        {
            Id = id,
            Variant = variant, //TODO ano/ne?
            Domain = Constants.PCP,
            Instance = instance ?? "",
            Resource = Constants.LoanSoldProduct,
        };

    public static ResourceIdentifier? CreateCollateralAgreement(string id)
        => new ResourceIdentifier
        {
            Id = id,
            Domain = Constants.CAM,
            Instance = Constants.MPSS,
            Resource = Constants.Collateral,
        };

    public static ResourceIdentifier CreateUser(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(0, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
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
