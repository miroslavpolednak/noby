﻿namespace NOBY.ApiContracts;

public partial class RealEstateValuationAddDeedOfOwnershipDocumentRequest
    : IRequest<int>
{
    [JsonIgnore]
    public long CaseId { get; private set; }

    [JsonIgnore]
    public int RealEstateValuationId { get; private set; }

    public RealEstateValuationAddDeedOfOwnershipDocumentRequest InfuseId(long caseId, int realEstateValuationId)
    {
        CaseId = caseId;
        RealEstateValuationId = realEstateValuationId;
        return this;
    }
}
