namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class MortgageRetentionData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SimulationInputsData SimulationInputs { get; set; }
    public SimulationOutputsData SimulationOutputs { get; set; }
    public BasicParametersData BasicParameters { get; set; }

    public sealed class SimulationInputsData
    {
        public DateTime InterestRateValidFrom { get; set; }
        public decimal InterestRate {  get; set; }
        public decimal? InterestRateDiscount { get; set; }
    }

    public sealed class SimulationOutputsData
    {
        public decimal LoanPaymentAmount { get; set; }
        public decimal? LoanPaymentAmountDiscounted { get; set; }
    }

    public sealed class BasicParametersData
    {
        public decimal Amount { get; set; }
        public decimal AmountIndividualPrice { get; set; }
    }
}
