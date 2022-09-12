using System.ComponentModel.DataAnnotations;

namespace FOMS.Api.Endpoints.Address.AddressSearch;

public class AddressSearchRequest
    : IRequest<AddressSearchResponse>
{
    /// <summary>
    /// SessionId generované přímo na frontendu (unikátní GUID)
    /// </summary>
    [Required]
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Pattern, podle kterého vyhledáváme
    /// </summary>
    [Required]
    public string SearchText { get; set; } = string.Empty;

    /// <summary>
    /// Id státu z číselníku Country
    /// </summary>
    public int? CountryId { get; set; }

    /// <summary>
    /// Počet vrácených záznamů
    /// </summary>
    [Required]
    public int PageSize { get; set; }
}
