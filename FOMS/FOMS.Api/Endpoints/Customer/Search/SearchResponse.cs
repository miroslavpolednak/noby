namespace FOMS.Api.Endpoints.Customer.Search;

public sealed class SearchResponse
{
    /// <summary>
    /// Informace o strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }
    
    /// <summary>
    /// Kolekce nalezenych klientu.
    /// </summary>
    public List<Dto.CustomerInList>? Rows { get; set; }
}