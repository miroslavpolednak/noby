namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

internal sealed class BuildingSavingsData : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SimulationInputsData SimulationInputs { get; set; } = null!;

    public SimulationOutputsData SimulationOutputs { get; set; } = null!;

    public sealed class SimulationInputsData
    {
        public int? MarketingActionCode { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal MinimumMonthlyDeposit { get; set; }
        public DateOnly ContractStartDate { get; set; }
        public bool SimulateUntilBindingPeriod { get; set; }
        public DateOnly? ContractTerminationDate { get; set; }
        public bool AnnualStatementRequired { get; set; }
        public bool StateSubsidyRequired { get; set; }
        public bool IsClientSVJ { get; set; }
        public bool IsClientJuridicalPerson { get; set; }
        public DateOnly ClientDateOfBirth { get; set; }
        public ICollection<ExtraDeposit> ExtraDeposits { get; set; } = [];

        public sealed class ExtraDeposit
        {
            public decimal Amount { get; set; }
            public DateOnly Date { get; set; }
        }
    }

    public sealed class SimulationOutputsData
    {
        public int SavingsLengthInMonths { get; set; }
        public decimal InterestRate { get; set; }
        public decimal SavingsSum { get; set; }
        public decimal DepositsSum { get; set; }
        public decimal InterestsSum { get; set; }
        public decimal FeesSum { get; set; }
        public decimal BonusInterestRate { get; set; }
        public decimal StateSubsidySum { get; set; }
        public decimal InterestBenefitAmount { get; set; }
        public decimal InterestBenefitTax { get; set; }
    }
}