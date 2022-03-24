using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("SalesArrangement", Schema = "dbo")]
internal class SalesArrangement : CIS.Core.Data.BaseCreatedWithModifiedUserId
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
}
