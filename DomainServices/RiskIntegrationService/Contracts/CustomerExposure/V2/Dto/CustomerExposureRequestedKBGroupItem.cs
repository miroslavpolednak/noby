﻿namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureRequestedKBGroupItem
{
    [ProtoMember(1)]
    public string? RiskBusinessCaseId { get; set; }

    [ProtoMember(2)]
    public int? LoanType { get; set; }

    [ProtoMember(3)]
    public int? LoanTypeCategory { get; set; }

    [ProtoMember(4)]
    public int? CustomerRoleId { get; set; }

    [ProtoMember(5)]
    public decimal? LoanAmount { get; set; }

    [ProtoMember(6)]
    public decimal? InstallmentAmount { get; set; }

    [ProtoMember(7)]
    public bool IsSecured { get; set; }

    [ProtoMember(8)]
    public string? StatusCode { get; set; }

    [ProtoMember(9)]
    public DateTime? RequestDate { get; set; }

    [ProtoMember(10)]
    public long? AppendixCode { get; set; }

    [ProtoMember(11)]
    public string? AppendixAccNbr { get; set; }
}
