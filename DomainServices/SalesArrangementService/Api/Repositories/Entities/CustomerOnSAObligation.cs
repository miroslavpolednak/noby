using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.SalesArrangementService.Api.Repositories.Entities;

[Table("CustomerOnSAObligation", Schema = "dbo")]
internal class CustomerOnSAObligation
    : CIS.Core.Data.BaseCreatedWithModifiedUserId
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerOnSAObligationId { get; set; }

    public int CustomerOnSAId { get; set; }

    public int ObligationTypeId { get; set; }

    public int? InstallmentAmount { get; set; }

    public int? LoanPrincipalAmount { get; set; }

    public int? CreditCardLimit { get; set; }

    public int? ObligationState { get; set; }

    public int? CreditorId { get; set; }

    public string? CreditorName { get; set; }

    public bool? CreditorIsExternal { get; set; }

    public int? CorrectionTypeId { get; set; }

    public int? InstallmentAmountCorrection { get; set; }

    public int? LoanPrincipalAmountCorrection { get; set; }

    public int? CreditCardLimitCorrection { get; set; }
}
