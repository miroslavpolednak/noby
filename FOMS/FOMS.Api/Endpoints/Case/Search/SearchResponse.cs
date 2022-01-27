namespace FOMS.Api.Endpoints.Case.Dto;

internal sealed class SearchResponse
{
    public CIS.Infrastructure.WebApi.Types.PaginationResponse? Pagination { get; set; }
    public List<CaseModel>? Rows { get; set; }
}
