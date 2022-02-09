using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Repositories.Entities;

[Table("Offer", Schema = "dbo")]
internal class Offer : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferId { get; set; }

    public Guid ResourceProcessId { get; set; }

    public int ProductTypeId { get; set; }

    public string? Inputs { get; set; }

    public string? Outputs { get; set; }
}