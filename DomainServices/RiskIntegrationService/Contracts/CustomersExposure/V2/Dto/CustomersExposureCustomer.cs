﻿namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract]
public class CustomersExposureCustomer
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
    public List<CustomersExposureExistingKBGroupItem>? ExistingKBGroupNaturalPersonExposures { get; set; }

    [ProtoMember(7)]
    public List<CustomersExposureExistingKBGroupItem>? ExistingKBGroupJuridicalPersonExposures { get; set; }

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
