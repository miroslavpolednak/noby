namespace NOBY.Api.Endpoints.Cases.SearchCases;

public sealed class SearchCasesResponse
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