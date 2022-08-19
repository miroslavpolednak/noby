namespace DomainServices.RiskIntegrationService.Api;

internal static class Helpers
{
    private static string[] _kbPersonSchemas = new[] { "KBAD", "DMID" };
    const string _kbInstanceName = "KBCZ";
    const string _mpInstanceName = "MPSS";
    private static string[] _kbGroupPersonValues = new[] { "KBAD", "MPAD" };

    public static bool IsDealerSchema(string? schemaToCheck)
        => string.IsNullOrEmpty(schemaToCheck) ? throw new CisArgumentException(0, "IsKbGroupPerson() input parameter is null", nameof(schemaToCheck)) : !_kbGroupPersonValues.Contains(schemaToCheck);

    public static string GetResourceIdentifierInstanceForDealer(string? identityScheme)
        => string.IsNullOrEmpty(identityScheme) ? throw new CisArgumentException(0, "GetResourceIdentifierInstanceForDealer() input parameter is null", nameof(identityScheme)) : _kbPersonSchemas.Contains(identityScheme) ? _kbInstanceName : _mpInstanceName;

    public static string GetResourceInstanceFromMandant(int mandantId)
        => mandantId == (int)CIS.Foms.Enums.Mandants.Kb ? "KBCZ" : "MPSS";

    public static TResponse? GetEnumFromString<TResponse>(string? enumValue, TResponse? defaultValue = default(TResponse?)) 
        where TResponse : struct
    {
        if (string.IsNullOrEmpty(enumValue)) return defaultValue;

        if (!Enum.TryParse(typeof(TResponse), enumValue, out object? outValue))
            throw new CisValidationException(0, $"Can't cast {nameof(TResponse)} '{enumValue}' to C4M enum");
        return (TResponse)outValue!;
    }

    public static TResponse GetRequiredEnumFromString<TResponse>(string enumValue)
        where TResponse : struct
    {
        if (!Enum.TryParse(typeof(TResponse), enumValue, out object? outValue))
            throw new CisValidationException(0, $"Can't cast {nameof(TResponse)} '{enumValue}' to C4M enum");
        return (TResponse)outValue!;
    }

    public static TResponse? GetEnumFromInt<TResponse>(int? id, TResponse? defaultValue = default(TResponse?))
        where TResponse : struct
        => id.HasValue ? GetEnumFromString<TResponse>(id.ToString(), defaultValue) : default(TResponse?);
}

