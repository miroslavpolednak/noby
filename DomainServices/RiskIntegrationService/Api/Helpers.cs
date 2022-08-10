namespace DomainServices.RiskIntegrationService.Api;

internal static class Helpers
{
    private static string[] _kbGroupPersonValues = new[] { "KBAD", "MPAD" };

    public static bool IsKbGroupPerson(string? schemaToCheck)
        => string.IsNullOrEmpty(schemaToCheck) ? throw new CisArgumentException(0, "IsKbGroupPerson() input parameter is null", nameof(schemaToCheck)) : _kbGroupPersonValues.Contains(schemaToCheck);
}
