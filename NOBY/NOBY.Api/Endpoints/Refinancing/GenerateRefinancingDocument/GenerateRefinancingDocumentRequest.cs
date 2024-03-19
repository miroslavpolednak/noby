using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.GenerateRefinancingDocument;

public class GenerateRefinancingDocumentRequest : IRequest
{
    [JsonIgnore]
    public long CaseId { get; set; }

    [JsonIgnore]
    public int SalesArrangementId { get; set; }

    /// <summary>
    /// Typ podpisu. 1 - Fyzicky poštou (Tlač na centrále), 2 - Moje banka, 3 - Elektronicky, 4 - Fyzicky na pobočce
    /// </summary>
    public required int SignatureTypeDetailId { get; set; }

    /// <summary>
    /// Typ Refinančního dokumentu. 1 - Retenční dodatek, 2 - Individuální sdělení, 3 - Hedgeový dodatek
    /// </summary>
    public required int RefinancingDocumentTypeId { get; set; }

    /// <summary>
    /// Nejzazší datum podpisu dokumentu
    /// </summary>
    public DateTime? SignatureDeadline { get; set; }

    internal GenerateRefinancingDocumentRequest InfuseCaseId(long caseId)
    {
        CaseId = caseId;
        return this;
    }

    internal GenerateRefinancingDocumentRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
