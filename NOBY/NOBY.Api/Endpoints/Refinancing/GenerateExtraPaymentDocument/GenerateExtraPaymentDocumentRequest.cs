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
    /// Způsoby předání podle číselníku <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=857276813">HandoverTypeDetail (SB CalculationDocuments Service enum).</a>
    /// </summary>
    [Required]
    public int HandoverTypeDetailId { get; set; }
    
    /// <summary>
    /// KBID klienta, kterému se bude předávat vygenerovaný dokument vyčíslení (a případné vygenerované dokumenty souhlasů)).
    /// </summary>
    public long? ClientKbId { get; set; }

    internal GenerateExtraPaymentDocumentRequest Infuse(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;

        return this;
    }
}