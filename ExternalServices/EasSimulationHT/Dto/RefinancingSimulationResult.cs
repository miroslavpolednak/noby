namespace ExternalServices.EasSimulationHT.Dto;

public sealed class RefinancingSimulationResult
{
    public decimal LoanPaymentAmount { get; set; }
    public int LoanPaymentsCount { get; set; }
    public DateTime MaturityDate { get; set; }
}
