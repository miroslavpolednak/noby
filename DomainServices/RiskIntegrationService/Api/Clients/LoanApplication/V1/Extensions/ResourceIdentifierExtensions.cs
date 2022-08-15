﻿namespace DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

internal partial class ResourceIdentifier
{
    public static ResourceIdentifier CreateId(long id, string variant)
#pragma warning disable CA1305 // Specify IFormatProvider
        => new ResourceIdentifier
        {
            Instance = "MPSS",
            Domain = "LA",
            Resource = "LoanApplication",
            Id = id.ToString(),
            Variant = variant
        };
#pragma warning restore CA1305 // Specify IFormatProvider

    public static ResourceIdentifier Create(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = Helpers.GetResourceIdentifierInstanceForDealer(humanUser.IdentityScheme),
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw new CisValidationException(0, $"Can not find Id for ResourceIdentifier {domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
        };
}
