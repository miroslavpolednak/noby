using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Database.Entities;

[Table("Application_Event", Schema = "bdp")]
public class ApplicationEvent
{
    public long Id { get; set; }

    [StringLength(16)]
    [Column("Account_Nbr", TypeName = "nchar(16)")]
    public string AccountNbr { get; set; } = null!;

    [StringLength(10)]
    [Column("Event_Type", TypeName = "nchar(10)")]
    public string? EventType { get; set; }

    [StringLength(10)]
    [Column("Event_Value", TypeName = "nvarchar(10)")]
    public string? EventValue { get; set; }

    [Column("Event_Date")]
    public DateTime? EventDate { get; set; }
}
