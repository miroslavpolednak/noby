namespace DomainServices.HouseholdService.Contracts;

public interface IObligation
{
    public int? ObligationTypeId { get; set; }
    public CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDecimal CreditCardLimit { get; set; }
    public CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDecimal LoanPrincipalAmount { get; set; }
    public CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDecimal InstallmentAmount { get; set; }
    public ObligationCreditor Creditor { get; set; }
}