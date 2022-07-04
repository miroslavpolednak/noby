namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

internal partial class ResourceIdentifier
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";

    public static ResourceIdentifier Create(string domain, string resource, RiskIntegrationService.Contracts.HumanUser humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = _kbPersonSchemas.Contains(humanUser.IdentityScheme) ? _kbInstanceName : _mpInstanceName,
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.Identity ?? throw new CisValidationException(0, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
        };

    public static ResourceIdentifier Create(string instance, string domain, string resource, string? id)
        => new ResourceIdentifier
        {
            Instance = instance,
            Domain = domain,
            Resource = resource,
            Id = id
        };
}
