﻿
namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public sealed class RiskBusinessCaseLoanAgreement
{
    [ProtoMember(1)]
    public int DistributionChannelId { get; set; }

    [ProtoMember(2)]
    public int SignatureTypeId { get; set; }
}
