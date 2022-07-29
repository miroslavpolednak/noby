namespace FOMS.Api.Endpoints.Codebooks.DeveloperSearch;

public sealed class DeveloperSearchRequest
    : IRequest<DeveloperSearchResponse>
{
    /// <summary>
    /// Text pro vyhledávání
    /// </summary>
    public string? SearchText { get; set; }

    /// <summary>
    /// Nastaveni strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}
