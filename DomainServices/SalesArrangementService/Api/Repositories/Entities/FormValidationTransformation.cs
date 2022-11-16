using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("FormValidationTransformation", Schema = "dbo")]
public class FormValidationTransformation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FormId { get; set; } = string.Empty; 

    public string Path { get; set; } = string.Empty;

    public string? Category { get; set; }

    public string Text { get; set; } = string.Empty;
}
