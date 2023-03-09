using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("UverNemovitost", Schema = "dbo")]
internal sealed class Loan2RealEstate
{
    public int UverId { get; set; }

    public int NemovitostId { get; set; }

    public int UcelKod { get; set; }
}
