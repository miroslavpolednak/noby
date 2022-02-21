namespace FOMS.Api.Endpoints.Case.Search;

public sealed class SearchResponse
{
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }
    public List<Dto.CaseModel>? Rows { get; set; }
}
