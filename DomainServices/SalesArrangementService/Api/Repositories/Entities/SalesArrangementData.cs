using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("SalesArrangementData", Schema = "dbo")]
internal class SalesArrangementData : CIS.Core.Data.BaseInsertUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementDataId { get; set; }

    public int SalesArrangementId { get; set; }

    public string Data { get; set; } = "";
}
