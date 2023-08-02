using CIS.Foms.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("RealEstateValuationOrder", Schema = "dbo")]
internal sealed class RealEstateValuationOrder
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    public int RealEstateValuationOrderId { get; set; }

    public RealEstateValuationOrderTypes RealEstateValuationOrderType { get; set; }
    public int RealEstateValuationId { get; set; }
    public byte[]? DataBin { get; set; }
    public string? Data { get; set; }
}
