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
        this.InstallmentAmount = parent.InstallmentAmount;
        this.InstallmentAmountConsolidated = parent.InstallmentAmountConsolidated;
        this.IsObligationCreditorExternal = parent.IsObligationCreditorExternal;
        this.CreditCardLimit = parent.CreditCardLimit;
        this.CreditCardLimitConsolidated = parent.CreditCardLimitConsolidated;
        this.LoanPrincipalAmount = parent.LoanPrincipalAmount;
        this.LoanPrincipalAmountConsolidated = parent.LoanPrincipalAmountConsolidated;
        this.ObligationTypeId = parent.ObligationTypeId;
        this.CustomerIndex = index;
    }
}
