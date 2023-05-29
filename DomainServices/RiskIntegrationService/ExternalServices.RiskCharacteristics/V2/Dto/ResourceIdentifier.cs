namespace DomainServices.RiskIntegrationService.ExternalServices.RiskCharacteristics.V2.Contracts;

public class ResourceIdentifier
    : ExternalServices.Dto.C4mResourceIdentifier
{
    public static ResourceIdentifier CreateResourceProcessId(string id)
        => new ResourceIdentifier
        {
            Instance = Constants.MPSS,
            Domain = Constants.OM,
            Resource = Constants.OfferInstance,
            Id = id
        };
    
    public static ResourceIdentifier CreateCustomerId(string id, string instance)
        => new ResourceIdentifier
        {
            Instance = instance,
            Domain = Constants.CM,
            Resource = Constants.Customer,
            Id = id
        };
}
