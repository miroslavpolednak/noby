using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerOnSAObligations", Schema = "dbo")]
internal class CustomerOnSAObligations
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAObligationsId { get; set; }

    public int CustomerOnSAId { get; set; }

    public string? Obligations { get; set; }
}
