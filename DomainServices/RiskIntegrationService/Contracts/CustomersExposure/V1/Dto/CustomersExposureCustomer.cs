namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;

[ProtoContract]
public class CustomersExposureCustomer
{
    [ProtoMember(1)]
    public int? CustomerId { get; set; }

    [ProtoMember(2)]
    public int? CustomerId2 { get; set; }

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
    public List<CustomersExposureRequestedKBGroupItem>? RequestedKBGroupNaturalPersonExposures { get; set; }

    [ProtoMember(9)]
    public List<CustomersExposureRequestedKBGroupItem>? RequestedKBGroupJuridicalPersonExposures { get; set; }

    [ProtoMember(10)]
    public List<CustomersExposureExistingCBCBItem>? ExistingCBCBNaturalPersonExposureItem { get; set; }

    [ProtoMember(11)]
    public List<CustomersExposureExistingCBCBItem>? ExistingCBCBJuridicalPersonExposureItem { get; set; }

    [ProtoMember(12)]
    public List<CustomersExposureRequestedCBCBItem>? RequestedCBCBNaturalPersonExposureItem { get; set; }

    [ProtoMember(13)]
    public List<CustomersExposureRequestedCBCBItem>? RequestedCBCBJuridicalPersonExposureItem { get; set; }
}
