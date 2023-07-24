using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("RealEstateValuationDetail", Schema = "dbo")]
public class RealEstateValuationDetail
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key]
    public int RealEstateValuationId { get; set; }

    public int? RealEstateSubtypeId { get; set; }

    public string? ACVRealEstateTypeId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string? LoanPurposeDetails { get; set; }
    public byte[]? LoanPurposeDetailsBin { get; set; }

    public string? SpecificDetail { get; set; }
    public byte[]? SpecificDetailBin { get; set; }
}
