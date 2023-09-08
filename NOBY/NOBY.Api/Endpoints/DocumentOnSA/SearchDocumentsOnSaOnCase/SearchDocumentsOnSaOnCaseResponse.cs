using NOBY.Dto.Signing;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseResponse
{
    public IReadOnlyCollection<SearchResponseItem> FormIds { get; set; } = null!;
}
