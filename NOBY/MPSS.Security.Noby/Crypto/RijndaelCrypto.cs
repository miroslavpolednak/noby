using System.Text;
using System.Security.Cryptography;

namespace MPSS.Security.Noby;

internal class RijndaelCrypto : ICrypto
{
#pragma warning disable SYSLIB0022 // Type or member is obsolete
    // env typ pro soucasnou instanci
    private Configuration config;

    public RijndaelCrypto(Configuration c)
    {
        config = c;
    }

    private ICryptoTransform GetEncryptor()
    {
        return CreateEncryptor(config.RijndealPassword, config.RijndaelPwdIterations, config.RijndaelStrength, config.RijndaelVector, config.RijndaelSalt);
    }

    private ICryptoTransform GetDecryptor()
    {
        return CreateDecryptor(config.RijndealPassword, config.RijndaelPwdIterations, config.RijndaelStrength, config.RijndaelVector, config.RijndaelSalt);
    }

    /// <summary>
    /// Vraci encryptor podle zadanych parametru.
    /// </summary>
    /// <param name="password">Heslo</param>
    /// <param name="passwordIterations">Pocet iteraci hashovani hesla.</param>
    /// <param name="strength">128,192,256</param>
    /// <param name="vector">Vektor kryptovani</param>
    /// <returns></returns>
    public static ICryptoTransform CreateEncryptor(string password, int passwordIterations, int strength, byte[] vector, byte[] salt)
    {
        PasswordDeriveBytes pwd = new PasswordDeriveBytes(password, salt, "SHA1", passwordIterations);
        byte[] keyBytes = pwd.GetBytes(strength / 8);

        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        return symmetricKey.CreateEncryptor(keyBytes, vector);
    }

    /// <summary>
    /// Vraci decryptor podle zadanych parametru.
    /// </summary>
    /// <param name="password">Heslo</param>
    /// <param name="passwordIterations">Pocet iteraci hashovani hesla.</param>
    /// <param name="strength">128,192,256</param>
    /// <param name="vector">Vektor kryptovani</param>
    /// <returns></returns>
    public static ICryptoTransform CreateDecryptor(string password, int passwordIterations, int strength, byte[] vector, byte[] salt)
    {
        PasswordDeriveBytes pwd = new PasswordDeriveBytes(password, salt, "SHA1", passwordIterations);
        byte[] keyBytes = pwd.GetBytes(strength / 8);
        RijndaelManaged symmetricKey = new RijndaelManaged();
        symmetricKey.Mode = CipherMode.CBC;
        return symmetricKey.CreateDecryptor(keyBytes, vector);
    }

    /// <summary>
    /// Kryptovani retezce.
    /// </summary>
    /// <param name="plainText">Bytove pole s textem v UTF8.</param>
    /// <param name="encryptor">Custom encryptor.</param>
    /// <returns></returns>
    public byte[] Encrypt(byte[] plainText, ICryptoTransform encryptor)
    {
        if (encryptor == null)
        {
            encryptor = GetEncryptor();
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainText, 0, plainText.Length);
                cryptoStream.FlushFinalBlock();

                return memoryStream.ToArray();
            }
        }
    }

    /// <summary>
    /// Kryptovani retezce.
    /// </summary>
    /// <param name="text">Bytove pole s textem v UTF8.</param>
    /// <returns>Kryptovany text v BASE64.</returns>
    public string Encrypt(byte[] text)
    {
        return Convert.ToBase64String(this.Encrypt(text, GetEncryptor()));
    }

    /// <summary>
    /// Kryptovani retezce.
    /// </summary>
    /// <param name="text">Text pro zakryptovani v UTF8.</param>
    /// <returns>Kryptovany text v BASE64.</returns>
    public string Encrypt(string text)
    {
        return this.Encrypt(text, GetEncryptor());
    }

    /// <summary>
    /// Kryptovani retezce.
    /// </summary>
    /// <param name="text">Text pro zakryptovani v UTF8.</param>
    /// <param name="encryptor">Custom encryptor.</param>
    /// <returns>Kryptovany text v BASE64.</returns>
    public string Encrypt(string text, ICryptoTransform encryptor)
    {
        return Convert.ToBase64String(Encrypt(UTF8Encoding.UTF8.GetBytes(text), encryptor));
    }

    /// <summary>
    /// Dekryptovani retezce
    /// </summary>
    /// <param name="text">Bytove pole se zakodovanym retezcem v UTF8.</param>
    /// <param name="decryptor">Custom decryptor.</param>
    /// <returns>Bytove pole s rozkodovanym retezcem v UTF8.</returns>
    public byte[] Decrypt(byte[] text, ICryptoTransform decryptor)
    {
        if (decryptor == null)
        {
            decryptor = GetDecryptor();
        }

        byte[] arr;
        using (MemoryStream memoryStream = new MemoryStream(text))
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                MemoryStream ms = new MemoryStream();
                int k;
                while ((k = cryptoStream.ReadByte()) >= 0)
                    ms.WriteByte((byte)k);
                arr = ms.ToArray();
            }
        }

        return arr;
    }

    /// <summary>
    /// Dekryptovani retezce
    /// </summary>
    /// <param name="text">Bytove pole se zakodovanym retezcem v UTF8.</param>
    /// <returns>Bytove pole s rozkodovanym retezcem v UTF8.</returns>
    public byte[] Decrypt(byte[] text)
    {
        return Decrypt(text, GetDecryptor());
    }

    /// <summary>
    /// Dekoduje vstupni pole.
    /// </summary>
    /// <param name="text">BASE64 zakryptovany retezec.</param>
    /// <returns>UTF8 retezec.</returns>
    public string Decrypt(string text)
    {
        return UTF8Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(text)));
    }
}
