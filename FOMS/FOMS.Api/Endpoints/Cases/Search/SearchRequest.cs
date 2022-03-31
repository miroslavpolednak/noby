namespace FOMS.Api.Endpoints.Cases.Search;

public class SearchRequest
    : IRequest<SearchResponse>
{
    /// <summary>
    /// [optional] ID pozadovaneho stavu filtru.
    /// </summary>
    public int? FilterId { get; set; }

    /// <summary>
    /// [optional] Klicove slovo pro vyhledavani nad Cases.
    /// </summary>
    public string? Term { get; set; }

    /// <summary>
    /// Nastaveni strankovani a razeni kolekce Cases.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}
