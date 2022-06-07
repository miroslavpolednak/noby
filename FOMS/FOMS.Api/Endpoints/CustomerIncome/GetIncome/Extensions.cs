using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

internal static class Extensions
{
    public static Dto.IncomeDataEmployement ToApiResponse(this _SA.IncomeDataEmployement contract)
        => new Dto.IncomeDataEmployement
        {
            ForeignIncomeTypeId = contract.ForeignIncomeTypeId,
            ProofOfIncomeToggle = contract.ProofOfIncomeToggle,
            WageDeductionToggle = contract.WageDeductionToggle,
            Employer = contract.Employer?.ToApiResponse(),
            IncomeConfirmation = contract.IncomeConfirmation?.ToApiResponse(),
            Job = contract.Job?.ToApiResponse(),
            WageDeduction = contract.WageDeduction?.ToApiResponse()
        };

    public static Dto.EmployerDataDto ToApiResponse(this _SA.EmployerData contract)
        => new Dto.EmployerDataDto
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            Name = contract.Name,
            CountryId = contract.CountryId
        };

    public static Dto.IncomeConfirmationDataDto ToApiResponse(this _SA.IncomeConfirmationData contract)
        => new Dto.IncomeConfirmationDataDto
        {
            ConfirmationByCompany = contract.ConfirmationByCompany,
            ConfirmationContact = contract.ConfirmationContact,
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson
        };

    public static Dto.JobDataDto ToApiResponse(this _SA.JobData contract)
        => new Dto.JobDataDto
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription,
            JobNoticePeriod = contract.JobNoticePeriod,
            JobTrialPeriod = contract.JobTrialPeriod,
            GrossAnnualIncome = contract.GrossAnnualIncome
        };

    public static Dto.WageDeductionDataDto ToApiResponse(this _SA.WageDeductionData contract)
        => new Dto.WageDeductionDataDto
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
