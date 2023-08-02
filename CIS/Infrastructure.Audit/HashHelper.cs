using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CIS.Infrastructure.Audit;

internal static class HashHelper
{
    public static string HashMessage(string message, byte[] secret)
    {
        // Převedeme klíč a zprávu na pole bajtů (byte[])
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Inicializujeme objekt HMAC-SHA256 s klíčem
        using (HMACSHA256 hmac = new HMACSHA256(secret))
        {
            // Spočítáme HMAC pro zprávu
            byte[] hmacBytes = hmac.ComputeHash(messageBytes);

            // Převedeme výsledek na šestnáctkový řetězec pro snadnější zobrazení
            return BitConverter
                .ToString(hmacBytes)
                .Replace("-", "")
                .ToLower(CultureInfo.InvariantCulture);
        }
    }
}
