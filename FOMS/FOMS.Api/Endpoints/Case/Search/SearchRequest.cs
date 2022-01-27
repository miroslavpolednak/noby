namespace FOMS.Api.Endpoints.Case.Dto;

internal class SearchRequest
    : IRequest<SearchResponse>
{
    public int? State { get; set; }
    public string? Term { get; set; }

    public CIS.Infrastructure.WebApi.Types.PaginationRequest? Pagination { get; set; }
}
