namespace FOMS.Api.Endpoints.Household.Dto;

public class CustomerObligation
{
    public int? ObligationTypeId { get; set; }
    public int? InstallmentAmount { get; set; }
    public int? InstallmentAmountConsolidated { get; set; }
    public int? LoanPrincipalAmount { get; set; }
    public int? LoanPrincipalAmountConsolidated { get; set; }
    public int? CreditCardLimit { get; set; }
    public int? CreditCardLimitConsolidated { get; set; }
    public bool IsObligationCreditorExternal { get; set; }
}
