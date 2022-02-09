namespace FOMS.Api.Endpoints.User.Dto;

internal class GetCurrentUserResponse
{
    /// <summary>
    /// v33id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Jmeno uzivatele
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Login
    /// </summary>
    public string? Username { get; set; }

    public string? CPM { get; set; }

    public string? ICP { get; set; }
}
