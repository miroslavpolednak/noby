using System.Globalization;
using System.Text;

namespace CIS.Infrastructure.ExternalServicesHelpers.Soap;
public static class NonceGenerator
{
    private static readonly Random _random = new Random();

    public static string GetNonce()
    {
        DateTime created = DateTime.Now;
        return Convert.ToBase64String(Encoding.ASCII.GetBytes(SHA256Encrypt(created + _random.Next().ToString(CultureInfo.CurrentCulture))));
    }

    // Helper method to hash the nonce (you can use a different hash function if needed)
    private static string SHA256Encrypt(string input)
    {
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(CultureInfo.CurrentCulture);
    }
}
