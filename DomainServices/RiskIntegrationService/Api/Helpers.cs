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
}
