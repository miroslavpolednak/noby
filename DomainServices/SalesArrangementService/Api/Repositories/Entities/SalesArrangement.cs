using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("SalesArrangement", Schema = "dbo")]
internal class SalesArrangement : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementId { get; set; }

    public long CaseId { get; set; }

    public int? OfferInstanceId { get; set; }

    public int SalesArrangementType { get; set; }

    public int State { get; set; }
}
