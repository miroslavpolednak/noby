using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("FormInstanceInterface", Schema = "dbo")]
internal class FormInstanceInterface
{
    [Key]
    public string DOKUMENT_ID { get; set; } = null!;

    public string? TYP_FORMULARE { get; set; }

    public string? CISLO_SMLOUVY { get; set; }
    
    public Int32? STATUS { get; set; }
    
    public char? DRUH_FROMULARE { get; set; }

    public string? FORMID { get; set; }
    
    public string? CPM { get; set; }

    public string? ICP { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public string? HESLO_KOD { get; set; }

    public Int32? STORNOVANO { get; set; }

    public Int32? TYP_DAT { get; set; }

    public string? JSON_DATA_CLOB { get; set; }
}
