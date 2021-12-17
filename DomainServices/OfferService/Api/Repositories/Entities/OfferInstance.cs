using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Repositories.Entities;

[Table("OfferInstance", Schema = "dbo")]
internal class OfferInstance : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferInstanceId { get; set; }

    public Guid ResourceProcessId { get; set; }

    public byte SimulationType { get; set; }

    public string? Inputs { get; set; }

    public string? OutputBuildingSavings { get; set; }

    public string? OutputBuildingSavingsLoan { get; set; }

    public string? OutputScheduleItems { get; set; }
}