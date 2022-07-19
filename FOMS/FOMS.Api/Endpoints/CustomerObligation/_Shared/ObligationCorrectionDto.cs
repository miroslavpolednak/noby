namespace FOMS.Api.Endpoints.CustomerObligation.Dto;

public class ObligationCorrectionDto
{
    public int? CorrectionTypeId { get; set; }

    public int? InstallmentAmountCorrection { get; set; }

    public int? LoanPrincipalAmountCorrection { get; set; }

    public int? CreditCardLimitCorrection { get; set; }
}
