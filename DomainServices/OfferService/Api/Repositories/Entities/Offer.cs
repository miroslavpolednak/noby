using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Repositories.Entities;

[Table("Offer", Schema = "dbo")]
internal class Offer : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferId { get; set; }

    public Guid ResourceProcessId { get; set; }

    public string BasicParameters { get; set; } = default!;

    public string SimulationInputs { get; set; } = default!;

    public string SimulationResults { get; set; } = default!;

    public byte[] BasicParametersBin { get; set; } = default!;

    public byte[] SimulationInputsBin { get; set; } = default!;

    public byte[] SimulationResultsBin { get; set; } = default!;

}