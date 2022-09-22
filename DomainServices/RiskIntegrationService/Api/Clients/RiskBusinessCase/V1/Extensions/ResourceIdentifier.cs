namespace DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V1.Contracts;

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

    public static ResourceIdentifier? CreateLoanSoldProduct(string id, string? instance)
        => new ResourceIdentifier
        {
            Id = id,
            Domain = Constants.PCP,
            Instance = instance ?? "",
            Resource = Constants.LoanSoldProduct,
        };

    public static ResourceIdentifier? CreateCollateralAgreement(string id)
        => new ResourceIdentifier
        {
            Id = id,
            Domain = Constants.CAM,
            Instance = Constants.KBCZ,
            Resource = Constants.Collateral,
        };

    public static ResourceIdentifier CreateUser(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(17000, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
        };
}
