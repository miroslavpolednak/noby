using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CaseService.Api.Database.Entities;

[Table("ConfirmedPriceException", Schema = "dbo")]
internal sealed class ConfirmedPriceException
{
    [Key]
    public int Id { get; set; }

    public long CaseId { get; set; }
    public int TaskIdSB { get; set; }
    public DateOnly? ConfirmedDate { get; set; }
    public DateOnly? DeclinedDate { get; set; }
    public DateTime CreatedTime { get; set; }
}
