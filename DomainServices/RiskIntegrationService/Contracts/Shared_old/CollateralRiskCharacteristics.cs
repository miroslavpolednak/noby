namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class CollateralRiskCharacteristics
{
    [ProtoMember(1)]
    public long? Ltv { get; set; }

    [ProtoMember(2)]
    public long? Ltfv { get; set; }

    [ProtoMember(3)]
    public long? Ltp { get; set; }

    [ProtoMember(4)]
    public long? SumAppraisedValue { get; set; }
}
