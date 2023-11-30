namespace DomainServices.HouseholdService.Contracts;

public interface IObligation
{
    public int? ObligationTypeId { get; set; }
    public SharedTypes.GrpcTypes.NullableGrpcDecimal CreditCardLimit { get; set; }
    public SharedTypes.GrpcTypes.NullableGrpcDecimal LoanPrincipalAmount { get; set; }
    public SharedTypes.GrpcTypes.NullableGrpcDecimal InstallmentAmount { get; set; }
    public SharedTypes.GrpcTypes.NullableGrpcDecimal AmountConsolidated { get; set; }
    public int? ObligationState { get; set; }
    public ObligationCreditor Creditor { get; set; }
    public ObligationCorrection Correction { get; set; }
}