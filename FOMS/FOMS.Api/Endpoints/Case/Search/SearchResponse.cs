namespace FOMS.Api.Endpoints.Case.Dto;

internal class SearchResponse
{
    public CIS.Core.Types.PaginableResponse? Pagination { get; set; }
    public List<SearchItem>? Rows { get; set; }
}
