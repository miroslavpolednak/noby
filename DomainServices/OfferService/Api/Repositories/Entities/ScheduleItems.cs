using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Repositories.Entities;

[Table("ScheduleItems", Schema = "dbo")]
internal class ScheduleItems
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ScheduleItemsId { get; set; }

    public int OfferInstanceId { get; set; }

    public string Data { get; set; } = "";
}
