using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.ProductService.Api.Database.Entities;

[Table("UverVypisy", Schema = "dbo")]
internal sealed class Loan2Statement
{
    [Key]
    public long Id { get; set; }

    public string? Ulice { get; set; }
    public string? CisloDomu4 { get; set; }
    public string? CisloDomu2 { get; set; }
    public string? Psc { get; set; }
    public string? Mesto { get; set; }
    public int? ZemeId { get; set; }
    public string? StatPodkategorie { get; set; }
}
