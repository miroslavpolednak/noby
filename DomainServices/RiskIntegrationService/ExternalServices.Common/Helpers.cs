
namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class Helpers
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";

    public static string GetResourceIdentifierInstanceForDealer(string? identityScheme)
        => string.IsNullOrEmpty(identityScheme) ? throw ErrorCodeMapper.CreateValidationException(ErrorCodeMapper.ResourceIdentifierIdIsEmpty) : _kbPersonSchemas.Contains(identityScheme) ? _kbInstanceName : _mpInstanceName;

}

