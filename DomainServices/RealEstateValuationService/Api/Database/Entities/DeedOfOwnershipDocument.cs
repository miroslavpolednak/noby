using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("DeedOfOwnershipDocument", Schema = "dbo")]
internal class DeedOfOwnershipDocument
    : CIS.Core.Data.BaseCreated
{
    [Key]
    public int DeedOfOwnershipDocumentId { get; set; }

    public long CremDeedOfOwnershipDocumentId { get; set; }
    public int RealEstateValuationId { get; set; }
    public int KatuzId { get; set; }
    public string? KatuzTitle { get; set; }
    public long DeedOfOwnershipId { get; set; }
    public int DeedOfOwnershipNumber { get; set; }
    public string? Address { get; set; }
    public long? AddressPointId { get; set; }
    public string? RealEstateIds { get; set; }
}
