using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("SalesArrangementParameters", Schema = "dbo")]
internal class SalesArrangementParameters
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesArrangementParametersId { get; set; }

    public int SalesArrangementId { get; set; }
    public string? Parameteres { get; set; }
}
