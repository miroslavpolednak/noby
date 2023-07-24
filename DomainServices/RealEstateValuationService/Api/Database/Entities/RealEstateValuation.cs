using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.RealEstateValuationService.Api.Database.Entities;

[Table("RealEstateValuation", Schema = "dbo")]
internal sealed class RealEstateValuation
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
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
    public int? OrderId { get; set; }
    public int RealEstateStateId { get; set; }
    public string? Address { get; set; }
    public DateTime? ValuationSentDate { get; set; }

    public RealEstateValuationDetail? Detail { get; set; }
}
