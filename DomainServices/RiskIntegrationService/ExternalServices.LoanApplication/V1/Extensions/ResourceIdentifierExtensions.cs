namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V1.Contracts;

public partial class ResourceIdentifier
{
    public static ResourceIdentifier CreateId(string id, string variant)
#pragma warning disable CA1305 // Specify IFormatProvider
        => new ResourceIdentifier
        {
            Instance = "MPSS",
            Domain = "LA",
            Resource = "LoanApplication",
            Id = id,
            Variant = variant
        };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static ResourceIdentifier Create(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(17000, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = resource == "Broker" ? humanUser.IdentityScheme! : null
        };
}
