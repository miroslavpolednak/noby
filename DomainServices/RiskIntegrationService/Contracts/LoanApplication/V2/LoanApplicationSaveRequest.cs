﻿namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationSaveRequest
    : IRequest<LoanApplicationSaveResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(2)]
    public int? DistributionChannelId { get; set; }

    [ProtoMember(3)]
    public SharedTypes.Enums.SignatureTypes SignatureType { get; set; }

    [ProtoMember(4)]
    public string LoanApplicationDataVersion { get; set; } = null!;

    [ProtoMember(5)]
    public List<LoanApplicationHousehold> Households { get; set; } = null!;

    [ProtoMember(6)]
    public LoanApplicationProduct Product { get; set; } = null!;

    [ProtoMember(7)]
    public List<LoanApplicationProductRelation>? ProductRelations { get; set; }

    [ProtoMember(8)]
    public List<LoanApplicationDeclaredSecuredProduct>? DeclaredSecuredProducts { get; set; }

    [ProtoMember(9)]
    public Shared.Identity? UserIdentity { get; set; }

    [ProtoMember(10)]
    public int? AppendixCode { get; set; }
}
