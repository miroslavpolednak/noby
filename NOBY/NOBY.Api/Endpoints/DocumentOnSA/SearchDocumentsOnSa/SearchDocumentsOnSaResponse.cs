using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.Search;

public class SearchDocumentsOnSaResponse
{
    public IReadOnlyCollection<SearchResponseItem> FormIds { get; set; } = null!;
}