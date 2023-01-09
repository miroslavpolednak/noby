using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Database.Entities;

[Table("FormValidationTransformation", Schema = "dbo")]
internal class FormValidationTransformation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FormId { get; set; } = string.Empty;

    public string FieldName { get; set; } = string.Empty;

    public string FieldPath { get; set; } = string.Empty;

    public string? Category { get; set; }

    public string Text { get; set; } = string.Empty;

    public FormValidationTransformationAlterSeverity AlterSeverity { get; set; }
}