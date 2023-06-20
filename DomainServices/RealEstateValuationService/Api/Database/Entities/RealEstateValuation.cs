using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("RealEstateValuation", Schema = "dbo")]
internal sealed class RealEstateValuation
{
    [Key]
    public int RealEstateValuationId { get; set; }
}
