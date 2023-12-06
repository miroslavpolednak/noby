using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Address.KatuzSearch;

public class KatuzSearchRequest : IRequest<KatuzSearchResponse>
{
    /// <summary>
    /// Pattern, podle kterého vyhledáváme
    /// </summary>
    [Required, MinLength(1)]
    public string SearchText { get; set; } = null!;

    /// <summary>
    /// Počet vrácených záznamů
    /// </summary>
    [Required]
    public int PageSize { get; set; }
}