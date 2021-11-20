namespace FOMS.Api.Endpoints.User.Dto;

internal sealed class GetCurrentUserResponse
{
    /// <summary>
    /// v33id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Jmeno uzivatele
    /// </summary>
    public string Name { get; set; } = "";
}
