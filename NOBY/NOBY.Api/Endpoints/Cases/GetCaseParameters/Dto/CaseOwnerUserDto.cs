using SharedTypes.Types;

namespace NOBY.Api.Endpoints.Cases.GetCaseParameters.Dto;

public sealed class CaseOwnerUserDto
{
    /// <summary>
    /// Pobočka/společnost třetí strany
    /// </summary>
    public string? BranchName { get; set; }

    /// <summary>
    /// Poradce (Jméno Příjmení)
    /// </summary>
    public string? ConsultantName { get; set; }

    /// <summary>
    /// ČPM
    /// </summary>
    public string? Cpm { get; set; }

    /// <summary>
    /// IČP
    /// </summary>
    public string? Icp { get; set; }

    /// <summary>
    /// Všechny identity uživatele z XXVVSS
    /// </summary>
    public List<UserIdentity> UserIdentifiers { get; set; } = null!;

    /// <summary>
    /// Flag, zda se jedná o interního uživatele, či externistu
    /// </summary>
    public bool IsInternal { get; set; }
}
