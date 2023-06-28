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

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public RealEstate RealEstate { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
