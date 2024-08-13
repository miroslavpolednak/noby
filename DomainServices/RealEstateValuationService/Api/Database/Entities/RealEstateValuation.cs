using DomainServices.RealEstateValuationService.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

[Table("RealEstateValuation", Schema = "dbo")]
internal sealed class RealEstateValuation
    : CIS.Core.Data.BaseCreatedWithModifiedUserId, IRealEstateValuationDetail
{
    [Key]
    public int RealEstateValuationId { get; set; }

    public long CaseId { get; set; }
    public int RealEstateTypeId { get; set; }
    public int ValuationStateId { get; set; }
    public bool IsLoanRealEstate { get; set; }
    public int ValuationTypeId { get; set; }
    public bool IsRevaluationRequired { get; set; }
    public bool DeveloperAllowed { get; set; }
    public bool DeveloperApplied { get; set; }
    public long? OrderId { get; set; }
    public int? RealEstateStateId { get; set; }
    public string? Address { get; set; }
    public DateTime? ValuationSentDate { get; set; }
    public long? PreorderId { get; set; }
    public int? RealEstateSubtypeId { get; set; }
    public string? ACVRealEstateTypeId { get; set; }
    public string? BagmanRealEstateTypeId { get; set; }
    public bool IsOnlineDisqualified { get; set; }
    public string? Comment { get; set; }
    public List<int>? PossibleValuationTypeId { get; set; }
    public List<PriceDetail>? Prices { get; set; }
}

internal sealed class PriceDetail
{
    public int Price { get; set; }
    public string PriceSourceType { get; set; }
}