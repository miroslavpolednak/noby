namespace FOMS.Api.Endpoints.Case.Search;

public class SearchRequest
    : IRequest<SearchResponse>
{
    /// <summary>
    /// ID pozadovaneho stavu Case. Ciselnik 'CaseStates'.
    /// </summary>
    public int? State { get; set; }
    
    /// <summary>
    /// Klicove slovo pro vyhledavani nad Cases.
    /// </summary>
    public string? Term { get; set; }

    /// <summary>
    /// Nastaveni strankovani a razeni kolekce Cases.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}
