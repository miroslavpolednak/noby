namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

public sealed class DeveloperSearchResponse
{
    /// <summary>
    /// Informace o strankovani a razeni.
    /// </summary>
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }

    /// <summary>
    /// Kolekce nalezenych projektu.
    /// </summary>
    public List<DomainServices.CodebookService.Contracts.v1.DeveloperSearchResponse.Types.DeveloperSearchItem>? Rows { get; set; }
}
