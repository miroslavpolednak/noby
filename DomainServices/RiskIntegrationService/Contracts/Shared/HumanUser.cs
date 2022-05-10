namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Uživatel (přihlášený uživatel/schvalovatel aplikace StarBuild/NOBY)
/// </summary>
public class HumanUser
{
    /// <summary>
    /// Identifikátor uživatele/schvalovatele (os.číslo, login, ...)
    /// </summary>
    public string? Identity { get; set; }

    /// <summary>
    /// Identitní schéma ("MPAD", "KBAD", "DMID" (BrokerId), "M04ID", "M17ID")
    /// </summary>
    public string? IdentityScheme { get; set; }
}
