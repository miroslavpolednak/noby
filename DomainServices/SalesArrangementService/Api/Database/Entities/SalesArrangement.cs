﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("SalesArrangement", Schema = "dbo")]
internal sealed class SalesArrangement : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementId { get; set; }

    public long CaseId { get; set; }

    public int? OfferId { get; set; }

    public Guid? ResourceProcessId { get; set; }

    public string? RiskBusinessCaseId { get; set; }

    public int SalesArrangementTypeId { get; set; }

    public int State { get; set; }

    public DateTime StateUpdateTime { get; set; }
    
    public string? ContractNumber { get; set; }
    
    public int ChannelId { get; set; }

    public string? LoanApplicationAssessmentId { get; set; }
    
    public string? RiskSegment { get; set; }

    public string? CommandId { get; set; }

    public DateTime? OfferGuaranteeDateFrom { get; set; }

    public DateTime? OfferGuaranteeDateTo { get; set; }

    public DateTime? FirstLoanAssessmentDate { get; set; }

    public DateTime? RiskBusinessCaseExpirationDate { get; set; }

    public DateTime? FirstSignatureDate { get; set; }

    public string? PcpId { get; set; }

    public string? LoanApplicationDataVersion { get; set; }

    public long? TaskProcessId { get; set; }
}
