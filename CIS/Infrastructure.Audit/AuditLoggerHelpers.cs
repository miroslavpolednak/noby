using System.Security.Cryptography;

namespace CIS.Infrastructure.Audit;

public static class AuditLoggerHelpers
{
    public static string GenerateSHA2(byte[] data)
    {
        var hashed = SHA512.HashData(data);
        return Convert.ToBase64String(hashed);
    }

    public static string GenerateSHA3(byte[] data)
    {
        return "";
    }
}
