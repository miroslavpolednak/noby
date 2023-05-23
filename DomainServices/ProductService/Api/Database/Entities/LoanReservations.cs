using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("RezervaceSmluv", Schema = "dbo")]
internal sealed class LoanReservation
{
    [Key]
    public long Id { get; set; }

    public long UverId { get; set; }

    public string? PcpInstId { get; set; }
}
