namespace FOMS.Api.Endpoints.Users.GetCurrentUser;

public class GetCurrentUserResponse
{
    /// <summary>
    /// ID uzivatele - xxvvss.v33id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Cele jmeno uzivatele
    /// </summary>
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// CPM uzivatele
    /// </summary>
    /// <example>99800001</example>
    public string? CPM { get; set; }

    /// <summary>
    /// ICP uzivatele
    /// </summary>
    /// <example>999999999</example>
    public string? ICP { get; set; }
}
