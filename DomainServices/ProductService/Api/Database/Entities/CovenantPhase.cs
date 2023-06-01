using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("TerminovnikFaze", Schema = "dbo")]
internal sealed class CovenantPhase
{
    [Column("Nazev")]
    public string Name { get; set; } = null!;
    
    [Column("Poradi")]
    public int Order { get; set; }

    [Column("PoradiPismeno")]
    public string OrderLetter { get; set; } = null!;
}