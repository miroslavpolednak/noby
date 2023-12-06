using NOBY.Api.Endpoints.Address.KatuzSearch.Dto;

namespace NOBY.Api.Endpoints.Address.KatuzSearch;

public class KatuzSearchResponse
{
    /// <summary>
    /// Počet vrácených záznamů
    /// </summary>
    public int PageSize { get; set; }

    public ICollection<KatuzLine> Rows { get; set; } = null!;
}