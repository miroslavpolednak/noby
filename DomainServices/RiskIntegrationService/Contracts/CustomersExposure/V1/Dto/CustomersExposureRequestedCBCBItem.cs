﻿namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;

[ProtoContract]
public class CustomersExposureRequestedCBCBItem
{
    [ProtoMember(1)]
    public string? CbcbContractId { get; set; }

    [ProtoMember(2)]
    public string? LoanType { get; set; }

    [ProtoMember(3)]
    public string? LoanTypeCategory { get; set; }

    [ProtoMember(4)]
    public DateTime? MaturityDate { get; set; }

    [ProtoMember(5)]
    public decimal? LoanAmount { get; set; }

    [ProtoMember(6)]
    public decimal? InstallmentAmount { get; set; }

    [ProtoMember(7)]
    public string? KbGroupInstanceCode { get; set; }

    [ProtoMember(8)]
    public DateTime? CbcbDataLastUpdate { get; set; }
}
