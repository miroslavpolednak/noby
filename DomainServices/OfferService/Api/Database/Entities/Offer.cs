using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.OfferService.Api.Database.Entities;

[Table("Offer", Schema = "dbo")]
internal sealed class Offer 
    : CIS.Core.Data.BaseCreated
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OfferId { get; set; }

    public long? CaseId { get; set; }

    public int? SalesArrangementId { get; set; }

    public DateTime? ValidTo { get; set; }

    public Guid ResourceProcessId { get; set; }

    public bool IsCreditWorthinessSimpleRequested { get; set; }

    public string? DocumentId { get; set; }

    public int OfferType { get; set; }

    public DateTime? FirstGeneratedDocumentDate { get; set; }

    public int Flags { get; set; }

    public int Origin { get; set; }
}