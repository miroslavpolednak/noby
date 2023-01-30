
using CIS.Core.Exceptions;

namespace DomainServices.RiskIntegrationService.ExternalServices;

public static class Helpers
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";
    private static string[] _kbGroupPersonValues = new[] { "KBAD", "MPAD" };

    public static string GetResourceIdentifierInstanceForDealer(string? identityScheme)
        => string.IsNullOrEmpty(identityScheme) ? throw new CisArgumentException(17012, "GetResourceIdentifierInstanceForDealer() input parameter is null", nameof(identityScheme)) : _kbPersonSchemas.Contains(identityScheme) ? _kbInstanceName : _mpInstanceName;

    public static TResponse GetRequiredEnumFromString<TResponse>(string enumValue)
        where TResponse : struct
    {
        if (!Enum.TryParse(typeof(TResponse), enumValue, out object? outValue))
            throw new CisValidationException(17013, $"Can't cast {typeof(TResponse)} '{enumValue}' to C4M enum");
        return (TResponse)outValue!;
    }
}

