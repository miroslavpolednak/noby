namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V3.Contracts;

public class ResourceIdentifier
    : ExternalServices.Dto.C4mResourceIdentifier
{
    public static ResourceIdentifier Create(string id, string? variant)
        => new ResourceIdentifier
        {
            Instance = Constants.MPSS,
            Domain = Constants.LA,
            Resource = Constants.LoanApplication,
            Id = id,
            Variant = variant
        };

    public static ResourceIdentifier Create(RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = Constants.BM,
            Resource = Constants.Broker,
            Id = id ?? humanUser.IdentityId ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ResourceIdentifierIdIsEmpty, ""),
            Variant = humanUser.IdentityScheme!
        };
}
