using System.Security.Claims;
using System.Security.Principal;

namespace MPSS.Security.Noby;

/// <summary>
/// Kontajner detailu uzivatele (ve vlastnosti Identity).
/// </summary>
[Serializable]
internal class MpssUser : ClaimsPrincipal
{
    public override IIdentity Identity => _Identity;
    private readonly IdentityBase _Identity;

    public MpssUser(IdentityBase identity) : base(identity)
    {
        _Identity = identity;
    }

    public int m17ID
    {
        get { return _Identity.m17ID; }
    }

    public int BrokerId
    {
        get { return _Identity.BrokerId; }
    }

    public string CPM
    {
        get { return _Identity.CPM; }
    }

    /// <summary>
    /// ICP uzivatele (pokud existuje).
    /// </summary>
    public string ICP
    {
        get { return _Identity.ICP; }
    }

    /// <summary>
    /// Jmeno a prijmeni uzivatele.
    /// </summary>
    public string Name
    {
        get { return _Identity.Name; }
    }

    /// <summary>
    /// Jednotne ID uzivatele (v33id).
    /// </summary>
    public int ID
    {
        get { return _Identity.ID; }
    }

    /// <summary>
    /// v03id uzivatele.
    /// </summary>
    public int LegacyV03ID
    {
        get { return _Identity.LegacyV03ID; }
    }

    /// <summary>
    /// v11id uzivatele.
    /// </summary>
    public int LegacyV11ID
    {
        get { return _Identity.LegacyV11ID; }
    }

    /// <summary>
    /// s53id uzivatele.
    /// </summary>
    public int LegacyS53ID
    {
        get { return _Identity.LegacyS53ID; }
    }
    
    /// <summary>
    /// Soucet prav uzivatele pro soucasnou aplikaci (podle Application ID).
    /// </summary>
    public long Permissions
    {
        get { return _Identity.Permissions; }
    }

    /// <summary>
    /// Impersonace
    /// </summary>
    public int Impersonate
    {
        get { return _Identity.Impersonate; }
    }
}
