using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("FormValidationTransformation", Schema = "dbo")]
internal sealed class FormValidationTransformation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FormId { get; set; } = string.Empty;

    public string FieldPath { get; set; } = string.Empty;

    public string? Category { get; set; }

    public int? CategoryOrder { get; set; }

    public string Text { get; set; } = string.Empty;

    public FormValidationTransformationAlterSeverity AlterSeverity { get; set; }
}