using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

public class GenerateExtraPaymentDocumentRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

    /// <summary>
    /// Způsob předání dokumentu vyčíslení klientovi. Číselník <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=857276813">HandoverTypeDetails</a>
    /// </summary>
    [Required]
    public int HandoverTypeDetailId { get; set; }
    
    /// <summary>
    /// KB ID klienta, kterému se bude předávat dokument vyčíslení.
    /// </summary>
    public long? ClientKbId { get; set; }

    internal GenerateExtraPaymentDocumentRequest Infuse(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;

        return this;
    }
}