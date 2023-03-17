using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("FormInstanceInterface", Schema = "dbo")]
internal class FormInstanceInterface
{
    [Key]
    public string DOCUMENT_ID { get; set; } = null!;

    public string? FORM_TYPE { get; set; }

    public short? STATUS { get; set; }

    public char? FORM_KIND { get; set; }

    public string? CPM { get; set; }

    public string? ICP { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public short? STORNO { get; set; }

    public short? DATA_TYPE { get; set; }

    public string? JSON_DATA_CLOB { get; set; }
}
