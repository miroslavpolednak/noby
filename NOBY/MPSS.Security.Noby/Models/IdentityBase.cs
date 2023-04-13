using System.Security.Principal;
using System.Text;

namespace MPSS.Security.Noby;

/// <summary>
/// <para>Kontajner s detailem prihlaseneho uzivatele.</para>
/// <para>Promenna _Switches se pouziva k ulozeni dulezitych hodnot, ktere by nemeli byt primo pristupne pro vyvojare.</para>
/// </summary>
[Serializable]
internal class IdentityBase : IIdentity
{
    #region internal properties
    internal string SessionId;
    /// <summary>
    /// Prvni vytvoreni security cookie
    /// </summary>
    internal DateTime _FirstLogin;

    /// <summary>
    /// Soucet prepinacu nastavenych pro konkretniho uzivatele
    /// </summary>
    internal int _Switches;

    /// <summary>
    /// Retezec obsahujici prava na vsechny zatim zadane aplikace.
    /// Retezec je ve formatu "{id_aplikace_1}={pravo};{id_aplikace_2}={pravo},{impersonate};...".
    /// </summary>
    private string AllStoredPermissions { get; set; }
    #endregion

    #region kesovani prav
    /// <summary>
    /// Id posledni aplikace, jejiz prava pro uzivatele byla ulozena v Permissions.
    /// Nastaveni prav aplikace pri posledni serializaci security cookie. Uklada se jako kesovani prav - vetsinou je nastaveni prav pro vice za sebou jdoucich pozadavku stejne (jedna se o stejnou aplikaci).
    /// </summary>
    private int LastApplicationId;

    /// <summary>
    /// Prava posledni aplikace, ktera byla ulozena v Permissions
    /// </summary>
    private long LastPermission;

    /// <summary>
    /// ID impersonisace pro posledni aplikaci.
    /// </summary>
    private int LastImpersonate;
    #endregion

    #region public properties
    public int m17ID
    {
        get;
        internal set;
    }

    public int BrokerId
    {
        get;
        internal set;
    }

    /// <summary>
    /// CPM uzivatele.
    /// </summary>
    public string CPM
    {
        get;
        internal set;
    }

    /// <summary>
    /// ICP uzivatele.
    /// </summary>
    public string ICP
    {
        get;
        internal set;
    }

    /// <summary>
    /// Jmeno a prijmeni uzivatele.
    /// </summary>
    public string Name
    {
        get;
        internal set;
    }

    /// <summary>
    /// Jednotne ID uzivatele (v33id).
    /// </summary>
    public int ID
    {
        get;
        internal set;
    }

    /// <summary>
    /// v03id uzivatele
    /// </summary>
    public int LegacyV03ID
    {
        get;
        internal set;
    }

    /// <summary>
    /// v11id uzivatele
    /// </summary>
    public int LegacyV11ID
    {
        get;
        internal set;
    }

    /// <summary>
    /// s53id_log uzivatele
    /// </summary>
    public int LegacyS53ID
    {
        get;
        internal set;
    }

    /// <summary>
    /// True pokud byl uzivatel uspesne autentikovan.
    /// </summary>
    public bool IsAuthenticated
    {
        get;
        internal set;
    }

    public DateTime PasswordExpiration
    {
        get;
        internal set;
    }

    /// <summary>
    /// Required by IIdenity - no use in app
    /// </summary>
    public string AuthenticationType
    {
        get { return "PMP"; }
    }
    #endregion

    #region serializace
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0}{1}", CPM, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", ICP, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", Name, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", ID, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", IsAuthenticated, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", PasswordExpiration.ToString(System.Globalization.CultureInfo.InvariantCulture), SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", SessionId, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LegacyV03ID, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LegacyV11ID, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LegacyS53ID, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", AllStoredPermissions, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LastApplicationId, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LastPermission, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", LastImpersonate, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", _FirstLogin.ToString(System.Globalization.CultureInfo.InvariantCulture), SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", _Switches, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}{1}", m17ID, SecurityCookie.Separator[0]);
        sb.AppendFormat("{0}", BrokerId);
        return sb.ToString();
    }

    internal static IdentityBase GetFromString(string value)
    {
        IdentityBase ident = new IdentityBase(true);
        string[] arr = value.Split(SecurityCookie.Separator);
        ident.CPM = arr[0];
        ident.ICP = arr[1];
        ident.Name = arr[2];
        ident.ID = Convert.ToInt32(arr[3]);
        ident.IsAuthenticated = Convert.ToBoolean(arr[4]);
        ident.SessionId = arr[6];
        ident.LegacyV03ID = Convert.ToInt32(arr[7]);
        ident.LegacyV11ID = Convert.ToInt32(arr[8]);
        ident.LegacyS53ID = Convert.ToInt32(arr[9]);
        ident.AllStoredPermissions = arr[10];
        ident.LastApplicationId = Convert.ToInt32(arr[11]);
        ident.LastPermission = Convert.ToInt32(arr[12]);
        ident.LastImpersonate = Convert.ToInt32(arr[13]);
        int switches;
        int.TryParse(arr[15], out switches);
        ident._Switches = switches;
        ident.m17ID = Convert.ToInt32(arr[16]);
        ident.BrokerId = Convert.ToInt32(arr[17]);

        try
        {
            ident._FirstLogin = Convert.ToDateTime(arr[14], System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception err)
        {
            throw new Exception("IdentityBase.GetFromString(): error converting _FirstLogin to DateTime from '" + arr[14] + "'", err);
        }
        try
        {
            ident.PasswordExpiration = Convert.ToDateTime(arr[5], System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception err)
        {
            throw new Exception("IdentityBase.GetFromString(): error converting PasswordExpiration to DateTime from '" + arr[5] + "'", err);
        }

        return ident;
    }
    #endregion

    /// <summary>
    /// Constructor
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public IdentityBase(bool isAuthenticated)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        this.IsAuthenticated = isAuthenticated;
        this._Switches = 0;
        this._FirstLogin = DateTime.Now;
    }

    #region nastaveni prav
    [NonSerialized]
    internal long _Permissions;
    [NonSerialized]
    private int _Impersonate;

    /// <summary>
    /// Pristupne mody pro uzivatele v soucasne aplikaci
    /// </summary>
    public long Permissions
    {
        get { return this._Permissions; }
    }

    public int Impersonate
    {
        get { return this._Impersonate; }
    }
    #endregion
}
