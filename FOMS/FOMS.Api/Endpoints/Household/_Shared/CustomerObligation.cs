namespace FOMS.Api.Endpoints.Household.Dto;

public class CustomerObligation
{
    public int? ObligationTypeId { get; set; }
    public int? LoanPaymentAmount { get; set; }
    public int? RemainingLoanPrincipal { get; set; }
    public int? CreditCardLimit { get; set; }
    public bool IsObligationCreditorExternal { get; set; }
}
