namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3.Contracts;

public class ResourceIdentifier
    : ExternalServices.Dto.C4mResourceIdentifier
{
    public static ResourceIdentifier CreateResourceProcessId(string id)
        => new ResourceIdentifier
        {
            Id = id,
            Instance = Constants.MPSS,
            Domain = Constants.OM,
            Resource = Constants.OfferInstance
        };

    public static ResourceIdentifier CreateResourceCounterParty(string id, string instance)
        => new ResourceIdentifier
        {
            Id = id,
            Instance = instance,
            Domain = Constants.CM,
            Resource = Constants.Customer
        };

    public static ResourceIdentifier CreateResourceIdentifier(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new()
        {
            Instance = ExternalServices.Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(17000, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = resource == "Broker" ? humanUser.IdentityScheme! : null
        };
}