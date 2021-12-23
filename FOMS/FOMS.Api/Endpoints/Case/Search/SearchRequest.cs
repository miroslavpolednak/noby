namespace FOMS.Api.Endpoints.Case.Dto;

internal class SearchRequest
    : CIS.Core.Types.PaginableRequest, IRequest<SearchResponse>
{
    public int? State { get; set; }
    public string? Term { get; set; }
}
