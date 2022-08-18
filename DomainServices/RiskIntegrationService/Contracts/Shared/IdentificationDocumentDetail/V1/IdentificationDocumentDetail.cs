namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public sealed class IdentificationDocumentDetail
{
    [ProtoMember(1)]
    public string? DocumentNumber { get; set; }

    [ProtoMember(2)]
    public int? IdentificationDocumentTypeId { get; set; }

    [ProtoMember(3)]
    public DateTime? IssuedOn { get; set; }

    [ProtoMember(4)]
    public DateTime? ValidTo { get; set; }
}
