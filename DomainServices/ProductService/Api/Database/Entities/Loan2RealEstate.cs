using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("UverNemovitost", Schema = "dbo")]
[Keyless]
internal sealed class Loan2RealEstate
{
    public long UverId { get; set; }

    public long NemovitostId { get; set; }

    public Int16 UcelKod { get; set; }

    public RealEstate RealEstate { get; set; }
}
