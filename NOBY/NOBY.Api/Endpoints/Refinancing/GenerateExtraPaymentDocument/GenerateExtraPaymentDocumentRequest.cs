using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

public class GenerateExtraPaymentDocumentRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

    public int HandoverTypeDetailId { get; set; }
    
    public int ClientKbId { get; set; }

    internal GenerateExtraPaymentDocumentRequest Infuse(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;

        return this;
    }
}