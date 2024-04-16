﻿using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.Refinancing.GenerateExtraPaymentDocument;

public class GenerateExtraPaymentDocumentRequest : IRequest
{
    [JsonIgnore]
    internal long CaseId { get; set; }

    [JsonIgnore]
    internal int SalesArrangementId { get; set; }

    public required int RefinancingDocumentTypeId { get; set; }

    /// <summary>
    /// Typ podpisu. 1 - Fyzicky poštou (Tlač na centrále), 2 - Moje banka, 3 - Elektronicky, 4 - Fyzicky na pobočce
    /// </summary>
    public required int SignatureTypeDetailId { get; set; }

    /// <summary>
    /// Nejzazší datum podpisu dokumentu
    /// </summary>
    public DateTime? SignatureDeadline { get; set; }

    internal GenerateExtraPaymentDocumentRequest Infuse(long caseId, int salesArrangementId)
    {
        CaseId = caseId;
        SalesArrangementId = salesArrangementId;

        return this;
    }
}