namespace MPSS.Security.Noby;

/// <summary>
/// Vraci instanci tridy pro kryptovani podle zadanych preferenci.
/// </summary>
internal static class CryptoFactory
{
    /// <summary>
    /// Vytvori instanci tridy pro kryptovani podle zadanych preferenci.
    /// </summary>
    public static ICrypto GetCryptoProvider(Configuration c)
    {
        return new RijndaelCrypto(c);
    }
}
