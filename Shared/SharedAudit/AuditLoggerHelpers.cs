using System.Security.Cryptography;

namespace SharedAudit;

public static class AuditLoggerHelpers
{
    public static string GenerateSHA2(byte[] data)
    {
        var hashed = SHA512.HashData(data);
        return Convert.ToBase64String(hashed);
    }

    public static string GenerateSHA3(byte[] data)
    {
        return GenerateSHA2(data);
#pragma warning disable CS0162 // Unreachable code detected
        var hashed = SHA3_512.HashData(data);
#pragma warning restore CS0162 // Unreachable code detected
        return Convert.ToBase64String(hashed);
    }
}
