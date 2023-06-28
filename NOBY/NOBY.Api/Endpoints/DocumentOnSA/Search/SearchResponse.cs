namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchResponse
{
    public IReadOnlyCollection<SearchResponseItem> FormIds { get; set; } = null!;
}

public class SearchResponseItem
{
    public string FormId { get; set; } = null!;
}