namespace NOBY.Api.Endpoints.Address.AddressSearch.Dto;

public sealed class AddressLine
{
    /// <summary>
    /// Textová reprezentace adresy
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Id adresy
    /// </summary>
    public string Id { get; set; } = String.Empty;
}
