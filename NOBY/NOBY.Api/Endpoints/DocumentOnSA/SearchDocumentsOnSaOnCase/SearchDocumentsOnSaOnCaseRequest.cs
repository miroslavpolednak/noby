using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.DocumentOnSA.SearchDocumentsOnSaOnCase;

public class SearchDocumentsOnSaOnCaseRequest : IRequest<SearchDocumentsOnSaOnCaseResponse>
{
    [JsonIgnore]
    public long CaseId { get; set; }

    public int? EACodeMainId { get; set; }

    internal SearchDocumentsOnSaOnCaseRequest InfuseCaseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }

}
