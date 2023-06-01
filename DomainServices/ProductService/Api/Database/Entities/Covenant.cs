using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Terminovnik", Schema = "dbo")]
internal sealed class Covenant
{
    [Column("TextNazevProKlienta")]
    public string Name { get; set; } = null!;
    
    [Column("PriznakSplnena")]
    public bool IsFulFilled { get; set; }
    
    [Column("SplnitDo")]
    public DateTime FulfillDate { get; set; }

    [Column("TypSmlouvaPoradiPismeno")]
    public string OrderLetter { get; set; } = null!;
    
    [Column("TypSmlouvy")]
    public int CovenantTypeId { get; set; }
    
    [Column("FazePoradi")]
    public int PhaseOrder { get; set; }
}