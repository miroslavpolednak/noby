using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Nemovitost", Schema = "dbo")]
internal sealed class RealEstate
{
    [Key]
    public long Id { get; set; }

    public int TypKod { get; set; }
}
