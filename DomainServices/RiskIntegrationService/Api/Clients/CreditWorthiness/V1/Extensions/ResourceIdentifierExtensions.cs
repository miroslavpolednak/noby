namespace DomainServices.RiskIntegrationService.Api.Clients.CreditWorthiness.V1.Contracts;

internal partial class ResourceIdentifier
{
    public static ResourceIdentifier Create(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(0, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
        };

    public static ResourceIdentifier CreateResourceProcessId(string? id)
        => new ResourceIdentifier
        {
            Instance = "MPSS",
            Domain = "OM",
            Resource = "OfferInstance",
            Id = id
        };
}

