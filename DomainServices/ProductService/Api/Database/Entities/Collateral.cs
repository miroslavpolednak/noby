using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Zabezpeceni", Schema = "dbo")]
internal sealed class Collateral
{
    [Key]
    public long Id { get; set; }

    public long UverId { get; set; }

    public long NemovitostId { get; set; }
}
