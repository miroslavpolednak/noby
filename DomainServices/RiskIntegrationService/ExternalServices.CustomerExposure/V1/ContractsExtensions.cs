﻿namespace DomainServices.RiskIntegrationService.ExternalServices.CustomerExposure.V1.Contracts;

public partial class ResourceIdentifier
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";

    public static ResourceIdentifier Create(string domain, string resource, RiskIntegrationService.Contracts.Shared.Identity humanUser, string? id = null)
        => new ResourceIdentifier
        {
            Instance = _kbPersonSchemas.Contains(humanUser.IdentityScheme) ? _kbInstanceName : _mpInstanceName,
            Domain = domain,
            Resource = resource,
            Id = id ?? humanUser.IdentityId ?? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ResourceIdentifierIdIsEmpty, $"{domain}/{resource}"),
            Variant = humanUser.IdentityScheme!
        };

    public static ResourceIdentifier Create(string instance, string domain, string resource, string? id, string variant)
        => new ResourceIdentifier
        {
            Instance = instance,
            Domain = domain,
            Resource = resource,
            Id = id,
            Variant = variant
        };
}
