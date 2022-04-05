namespace FOMS.Api.Endpoints.Household.Dto;

public class HouseholdCustomerObligation
    : CustomerObligation
{
    /// <summary>
    /// Cislo klienta (1 nebo 2)
    /// </summary>
    public int CustomerIndex { get; set; }

    public HouseholdCustomerObligation() { }
    
    internal HouseholdCustomerObligation(CustomerObligation parent, int index)
    {
        this.ObligationCreditor = parent.ObligationCreditor;
        this.CreditCardLimit = parent.CreditCardLimit;
        this.RemainingLoanPrincipal = parent.RemainingLoanPrincipal;
        this.LoanPaymentAmount = parent.LoanPaymentAmount;
        this.ObligationTypeId = parent.ObligationTypeId;
        this.CustomerIndex = index;
    }
}
