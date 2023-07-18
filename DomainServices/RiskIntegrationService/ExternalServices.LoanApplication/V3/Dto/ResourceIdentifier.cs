namespace DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V3.Contracts;

public class ResourceIdentifier
    : ExternalServices.Dto.C4mResourceIdentifier
{
    public static ResourceIdentifier CreateId(string id, string variant)
#pragma warning disable CA1305 // Specify IFormatProvider
        => new ResourceIdentifier
        {
            Instance = Constants.MPSS,
            Domain = Constants.LA,
            Resource = Constants.LoanApplication,
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
            Id = id ?? humanUser.IdentityId ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ResourceIdentifierIdIsEmpty, $"{domain}/{resource}"),
            Variant = resource == "Broker" ? humanUser.IdentityScheme! : null
        };
}
