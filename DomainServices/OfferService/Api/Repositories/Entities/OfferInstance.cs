using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Repositories.Entities;

[Table("OfferInstance", Schema = "dbo")]
internal class OfferInstance : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferInstanceId { get; set; }

    public Guid ResourceProcessId { get; set; }

    public int ProductInstanceTypeId { get; set; }

    public string? Inputs { get; set; }

    public string? Outputs { get; set; }
}