using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Codebooks.DeveloperSearch;

public sealed class DeveloperSearchRequest
    : IRequest<DeveloperSearchResponse>
{
    /// <summary>
    /// Text pro vyhledávání
    /// </summary>
    [Required]
    public string? SearchText { get; set; }

    /// <summary>
    /// Nastaveni strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}
