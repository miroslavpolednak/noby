using DomainServices.RiskIntegrationService.ExternalServices;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1.Contracts;

internal partial class ResourceIdentifier
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
}