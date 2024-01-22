using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Database.Entities;

[Table("Offer", Schema = "dbo")]
internal sealed class Offer 
    : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferId { get; set; }

    public Guid ResourceProcessId { get; set; }

    public bool IsCreditWorthinessSimpleRequested { get; set; }

    public string? DocumentId { get; set; }

    public DateTime? FirstGeneratedDocumentDate { get; set; }
}