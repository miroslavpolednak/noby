namespace NOBY.Api.Endpoints.Users.GetLoggedInUser;

public sealed class GetLoggedInUserResponse
{
    /// <summary>
    /// ID uzivatele - V33ID
    /// </summary>
    /// <example>61958</example>
    public int UserId { get; set; }

    /// <summary>
    /// Všechny identity uživatele z XXVVSS
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public List<CIS.Foms.Types.UserIdentity> UserIdentifiers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Základní informace o přihlášeném uživateli
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public GetLoggedInUserResponseUserInfo UserInfo { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public int[]? UserPermissions { get; set; }
}

public sealed class GetLoggedInUserResponseUserInfo
{
    /// <summary>
    /// Jméno uživatele
    /// </summary>
    /// <example>Jaroslav</example>
    public string? FirstName { get; set; }

    /// <summary>
    /// Přijmení uživatele
    /// </summary>
    /// <example>Cimrman</example>
    public string? LastName { get; set; }

    /// <summary>
    /// ČPM uživatele
    /// </summary>
    /// <example>99800001</example>
    public string? Cpm { get; set; }

    /// <summary>
    /// IČP uživatele
    /// </summary>
    /// <example>999999999</example>
    public string? Icp { get; set; }

    /// <summary>
    /// IČO uživatele
    /// </summary>
    /// <example>12345678</example>
    public string? Cin { get; set; }

    /// <summary>
    /// Telefon uživatele
    /// </summary>
    /// <example>+420 601 234 567</example>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Emailová adresa uživatele
    /// </summary>
    /// <example>jarda@cimrman.cz</example>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// VIP flag uživatele
    /// </summary>
    public bool IsUserVIP { get; set; }

    /// <summary>
    /// Flag, zda se jedná o interního uživatele, či externistu
    /// </summary>
    public bool IsInternal { get; set; }
}
