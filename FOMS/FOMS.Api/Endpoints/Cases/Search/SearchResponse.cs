namespace FOMS.Api.Endpoints.Cases.Search;

public sealed class SearchResponse
{
    /// <summary>
    /// Informace o strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }
    
    /// <summary>
    /// Kolekce nalezenych Case-s.
    /// </summary>
    public List<Dto.CaseModel>? Rows { get; set; }
}