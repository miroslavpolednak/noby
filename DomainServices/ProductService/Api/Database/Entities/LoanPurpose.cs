using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("UverUcely", Schema = "dbo")]
[Keyless]
internal sealed class LoanPurpose
{
    public int UverId { get; set; }

    public int UcelUveruId { get; set; }

    public long ZmNavrhId { get; set; }

    public bool? HlavniUcel { get; set; }

    [Column(TypeName = "decimal(16, 4)")]
    [Precision(16, 4)]
    public decimal? SumaUcelu { get; set; }
}
