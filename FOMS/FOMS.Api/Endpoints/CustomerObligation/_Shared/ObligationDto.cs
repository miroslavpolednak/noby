namespace FOMS.Api.Endpoints.CustomerObligation.Dto;

public abstract class ObligationDto
{
    public int? ObligationTypeId { get; set; }

    public int? InstallmentAmount { get; set; }

    public int? LoanPrincipalAmount { get; set; }

    public int? CreditCardLimit { get; set; }

    public int? ObligationState { get; set; }

    public ObligationCreditorDto? Creditor { get; set; }

    public ObligationCorrectionDto? Correction { get; set; }
}
