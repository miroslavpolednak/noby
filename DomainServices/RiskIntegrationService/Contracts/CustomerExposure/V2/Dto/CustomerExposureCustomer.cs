namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureCustomer
{
    [ProtoMember(1)]
    public long? InternalCustomerId { get; set; }

    [ProtoMember(2)]
    public string? PrimaryCustomerId { get; set; }

    [ProtoMember(3)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(4)]
    public bool CbcbRegisterCalled { get; set; }

    [ProtoMember(5)]
    public string? CbcbReportId { get; set; }

    [ProtoMember(6)]
    public List<CustomerExposureExistingKBGroupItem>? ExistingKBGroupNaturalPersonExposures { get; set; }

    [ProtoMember(7)]
    public List<CustomerExposureExistingKBGroupItem>? ExistingKBGroupJuridicalPersonExposures { get; set; }

    [ProtoMember(8)]
    public List<CustomerExposureRequestedKBGroupItem>? RequestedKBGroupNaturalPersonExposures { get; set; }

    [ProtoMember(9)]
    public List<CustomerExposureRequestedKBGroupItem>? RequestedKBGroupJuridicalPersonExposures { get; set; }

    [ProtoMember(10)]
    public List<CustomerExposureExistingCBCBItem>? ExistingCBCBNaturalPersonExposureItem { get; set; }

    [ProtoMember(11)]
    public List<CustomerExposureExistingCBCBItem>? ExistingCBCBJuridicalPersonExposureItem { get; set; }

    [ProtoMember(12)]
    public List<CustomerExposureRequestedCBCBItem>? RequestedCBCBNaturalPersonExposureItem { get; set; }

    [ProtoMember(13)]
    public List<CustomerExposureRequestedCBCBItem>? RequestedCBCBJuridicalPersonExposureItem { get; set; }
}
