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
    public int? ValuationResultFuturePrice { get; set; }
    public int? ValuationResultCurrentPrice { get; set; }
    public long? OrderId { get; set; }
    public int? RealEstateStateId { get; set; }
    public string? Address { get; set; }
    public DateTime? ValuationSentDate { get; set; }
    public long? PreorderId { get; set; }
    public int? RealEstateSubtypeId { get; set; }
    public string? ACVRealEstateTypeId { get; set; }
    public string? BagmanRealEstateTypeId { get; set; }
    public string? LoanPurposeDetails { get; set; }
    public byte[]? LoanPurposeDetailsBin { get; set; }
    public string? SpecificDetail { get; set; }
    public byte[]? SpecificDetailBin { get; set; }
    // zatim dam jako samostatne pole na Json data, ale casem sloucit treba s LoanPurposeDetails a nejakymi dalsimi poli, ktera pribudou
    // tezko rict, kdyz nevime jak ten objekt ma ve finale vypadat a dostavame zadani po kouskach
    public string? Documents { get; set; }
    public bool IsOnlineDisqualified { get; set; }
}
