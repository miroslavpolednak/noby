namespace DomainServices.RiskIntegrationService.Contracts;

/// <summary>
/// Uživatel (přihlášený uživatel/schvalovatel aplikace StarBuild/NOBY)
/// </summary>
[DataContract]
public class HumanUser
{
    /// <summary>
    /// Identifikátor uživatele/schvalovatele (os.číslo, login, ...)
    /// </summary>
    [DataMember(Order = 1)]
    public string? Identity { get; set; }

    /// <summary>
    /// Identitní schéma ("MPAD", "KBAD", "DMID" (BrokerId), "M04ID", "M17ID")
    /// </summary>
    [DataMember(Order = 2)]
    public string? IdentityScheme { get; set; }
}
