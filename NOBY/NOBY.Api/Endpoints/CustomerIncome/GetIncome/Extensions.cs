using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

internal static class Extensions
{
    public static SharedDto.IncomeDataOther ToApiResponse(this _HO.IncomeDataOther contract)
        => new SharedDto.IncomeDataOther
        {
            IncomeOtherTypeId = contract.IncomeOtherTypeId
        };

    public static SharedDto.IncomeDataEntrepreneur ToApiResponse(this _HO.IncomeDataEntrepreneur contract)
        => new SharedDto.IncomeDataEntrepreneur
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            CountryOfResidenceId = contract.CountryOfResidenceId
        };

    public static SharedDto.IncomeDataEmployement ToApiResponse(this _HO.IncomeDataEmployement contract)
        => new SharedDto.IncomeDataEmployement
        {
            ForeignIncomeTypeId = contract.ForeignIncomeTypeId,
            HasProofOfIncome = contract.HasProofOfIncome,
            HasWageDeduction = contract.HasWageDeduction,
            Employer = contract.Employer?.ToApiResponse(),
            IncomeConfirmation = contract.IncomeConfirmation?.ToApiResponse(),
            Job = contract.Job?.ToApiResponse(),
            WageDeduction = contract.WageDeduction?.ToApiResponse()
        };

    public static SharedDto.EmployerDataDto ToApiResponse(this _HO.EmployerData contract)
        => new SharedDto.EmployerDataDto
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            Name = contract.Name,
            CountryId = contract.CountryId
        };

    public static SharedDto.IncomeConfirmationDataDto ToApiResponse(this _HO.IncomeConfirmationData contract)
        => new SharedDto.IncomeConfirmationDataDto
        {
            IsIssuedByExternalAccountant = contract.IsIssuedByExternalAccountant,
            ConfirmationContact = contract.ConfirmationContact is null ? null : new()
            {
                PhoneIDC = contract.ConfirmationContact.PhoneIDC,
                PhoneNumber = contract.ConfirmationContact.PhoneNumber
            },
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson
        };

    public static SharedDto.JobDataDto ToApiResponse(this _HO.JobData contract)
        => new SharedDto.JobDataDto
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription,
            IsInProbationaryPeriod = contract.IsInProbationaryPeriod,
            IsInTrialPeriod = contract.IsInTrialPeriod
        };

    public static SharedDto.WageDeductionDataDto ToApiResponse(this _HO.WageDeductionData contract)
        => new SharedDto.WageDeductionDataDto
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
