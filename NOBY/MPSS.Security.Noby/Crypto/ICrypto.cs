namespace MPSS.Security.Noby;

/// <summary>
/// Rozhrani kryptovaci sluzby.
/// </summary>
internal interface ICrypto
{
    /// <summary>
    /// Zakodovat reteze.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Zakodovany BASE64 retezec.</returns>
    string Encrypt(string text);

    /// <summary>
    /// Zakodovat reteze.
    /// </summary>
    /// <param name="text">Vstupni pole.</param>
    /// <returns>Zakodovany BASE64 retezec.</returns>
    string Encrypt(byte[] text);

    /// <summary>
    /// Dekoduje vstupni pole.
    /// </summary>
    /// <param name="text">Vstupni pole bytu.</param>
    /// <returns>Dekodovane pole bytu.</returns>
    byte[] Decrypt(byte[] text);

    /// <summary>
    /// Dekoduje vstupni pole.
    /// </summary>
    /// <param name="text">BASE64 zakryptovany retezec.</param>
    /// <returns>UTF8 retezec.</returns>
    string Decrypt(string text);
}
