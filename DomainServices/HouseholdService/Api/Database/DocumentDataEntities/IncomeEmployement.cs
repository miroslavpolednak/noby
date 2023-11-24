namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities;

internal sealed class IncomeEmployement
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public int? ForeignIncomeTypeId { get; set; }
    public bool HasProofOfIncome { get; set; }
    public EmployerData? Employer { get; set; }
    public JobData? Job { get; set; }
    public bool HasWageDeduction { get; set; }
    public WageDeductionData? WageDeduction { get; set; }
    public IncomeConfirmationData? IncomeConfirmation { get; set; }

    internal sealed class EmployerData
    {
        public string? Name { get; set; }
        public string? BirthNumber { get; set; }
        public string? Cin { get; set; }
        public int? CountryId { get; set; }
    }

    internal sealed class JobData
    {
        public decimal? GrossAnnualIncome { get; set; }
        public string? JobDescription { get; set; }
        public bool IsInProbationaryPeriod { get; set; }
        public bool IsInTrialPeriod { get; set; }
        public int? EmploymentTypeId { get; set; }
        public DateTime? CurrentWorkContractSince { get; set; }
        public DateTime? CurrentWorkContractTo { get; set; }
        public DateTime? FirstWorkContractSince { get; set; }
    }

    internal sealed class WageDeductionData
    {
        public decimal? DeductionDecision { get; set; }
        public decimal? DeductionPayments { get; set; }
        public decimal? DeductionOther { get; set; }
    }

    internal sealed class IncomeConfirmationData
    {
        public bool IsIssuedByExternalAccountant { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string? ConfirmationPerson { get; set; }
        public ConfirmationContactDto? ConfirmationContact { get; set; }
    }

    internal sealed class ConfirmationContactDto
    {
        public string? PhoneNumber { get; set; }
        public string? PhoneIDC { get; set;  }
    }
}
