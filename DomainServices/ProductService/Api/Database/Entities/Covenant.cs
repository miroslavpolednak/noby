using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("Terminovnik", Schema = "dbo")]
public class Covenant
{
    [Column("UverId")]
    public long CaseId { get; set; }
    
    [Column("PoradoveCislo")]
    public int Order { get; set; }
    
    [Column("TextNazevProKlienta")]
    public string Name { get; set; } = null!;

    [Column("TextVysvetlujiciDokument")]
    public string Description { get; set; } = null!;

    [Column("TextDoUveroveSmlouvy")]
    public string Text { get; set; } = null!;
    
    [Column("PriznakSplnena")]
    public short IsFulFilled { get; set; }
    
    [Column("SplnitDo")]
    public DateTime? FulfillDate { get; set; }

    [Column("TypSmlouvaPoradiPismeno")]
    public string OrderLetter { get; set; } = null!;
    
    [Column("TypSmlouvy")]
    public int CovenantTypeId { get; set; }
    
    [Column("FazePoradi")]
    public short PhaseOrder { get; set; }
}