using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal static class Extensions
{
    public static Dto.IncomeDataOther ToApiResponse(this _HO.IncomeDataOther contract)
        => new Dto.IncomeDataOther
        {
            IncomeOtherTypeId = contract.IncomeOtherTypeId
        };

    public static Dto.IncomeDataEntrepreneur ToApiResponse(this _HO.IncomeDataEntrepreneur contract)
        => new Dto.IncomeDataEntrepreneur
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            CountryOfResidenceId = contract.CountryOfResidenceId
        };

    public static Dto.IncomeDataEmployement ToApiResponse(this _HO.IncomeDataEmployement contract)
        => new Dto.IncomeDataEmployement
        {
            ForeignIncomeTypeId = contract.ForeignIncomeTypeId,
            HasProofOfIncome = contract.HasProofOfIncome,
            HasWageDeduction = contract.HasWageDeduction,
            Employer = contract.Employer?.ToApiResponse(),
            IncomeConfirmation = contract.IncomeConfirmation?.ToApiResponse(),
            Job = contract.Job?.ToApiResponse(),
            WageDeduction = contract.WageDeduction?.ToApiResponse()
        };

    public static Dto.EmployerDataDto ToApiResponse(this _HO.EmployerData contract)
        => new Dto.EmployerDataDto
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            Name = contract.Name,
            CountryId = contract.CountryId
        };

    public static Dto.IncomeConfirmationDataDto ToApiResponse(this _HO.IncomeConfirmationData contract)
        => new Dto.IncomeConfirmationDataDto
        {
            IsIssuedByExternalAccountant = contract.IsIssuedByExternalAccountant,
            ConfirmationContact = contract.ConfirmationContact,
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson
        };

    public static Dto.JobDataDto ToApiResponse(this _HO.JobData contract)
        => new Dto.JobDataDto
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription,
            IsInProbationaryPeriod = contract.IsInProbationaryPeriod,
            IsInTrialPeriod = contract.IsInTrialPeriod,
            GrossAnnualIncome = contract.GrossAnnualIncome
        };

    public static Dto.WageDeductionDataDto ToApiResponse(this _HO.WageDeductionData contract)
        => new Dto.WageDeductionDataDto
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
