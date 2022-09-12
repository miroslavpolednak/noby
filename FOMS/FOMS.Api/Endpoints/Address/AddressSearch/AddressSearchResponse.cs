namespace FOMS.Api.Endpoints.Address.AddressSearch;

public sealed class AddressSearchResponse
{
    /// <summary>
    /// Počet vrácených záznamů
    /// </summary>
    public int PageSize { get; set; }

    public List<Dto.AddressLine>? Rows { get; set; }
}
