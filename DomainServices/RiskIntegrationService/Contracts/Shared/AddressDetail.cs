namespace DomainServices.RiskIntegrationService.Contracts.Shared;

[ProtoContract]
public class AddressDetail
{
    [ProtoMember(1)]
    public string? Street { get; set; }

    [ProtoMember(2)]
    public string? StreetNumber { get; set; }

    [ProtoMember(3)]
    public string? EvidenceNumber { get; set; }

    [ProtoMember(4)]
    public string? HouseNumber { get; set; }

    [ProtoMember(5)]
    public string? City { get; set; }

    [ProtoMember(6)]
    public int? CountryId { get; set; }

    [ProtoMember(7)]
    public string? Postcode { get; set; }

    [ProtoMember(8)]
    public int? RegionCode { get; set; }
}
