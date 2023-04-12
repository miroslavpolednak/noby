namespace MPSS.Security.Noby;

internal sealed class Configuration
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string RijndealPassword { get; set; }
    public string RijndaelVector { get; set; }
    public int RijndaelStrength { get; set; }
    public int RijndaelPwdIterations { get; set; }
    public string RijndaelSalt { get; set; }
    public string CryptoProvider { get; set; } = "RIJNDAEL";
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
