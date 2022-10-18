using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.HouseholdService.Api.Repositories.Entities;

[Table("CustomerOnSAObligation", Schema = "dbo")]
internal class CustomerOnSAObligation
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAObligationId { get; set; }

    public int CustomerOnSAId { get; set; }

    public int ObligationTypeId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? InstallmentAmount { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? LoanPrincipalAmount { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? CreditCardLimit { get; set; }

    public int? ObligationState { get; set; }

    public string? CreditorId { get; set; }

    public string? CreditorName { get; set; }

    public bool? CreditorIsExternal { get; set; }

    public int? CorrectionTypeId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? InstallmentAmountCorrection { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? LoanPrincipalAmountCorrection { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? CreditCardLimitCorrection { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    [Precision(12, 2)]
    public decimal? AmountConsolidated { get; set; }
}
