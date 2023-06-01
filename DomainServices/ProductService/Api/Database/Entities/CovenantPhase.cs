using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("TerminovnikFaze", Schema = "dbo")]
public class CovenantPhase
{
    [Column("UverId")]
    public long CaseId { get; set; }
    
    [Column("Nazev")]
    public string Name { get; set; } = null!;
    
    [Column("Poradi")]
    public short Order { get; set; }

    [Column("PoradiPismeno")]
    public string OrderLetter { get; set; } = null!;
}