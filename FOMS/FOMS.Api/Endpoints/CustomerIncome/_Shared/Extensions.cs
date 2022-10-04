using _HO = DomainServices.HouseholdService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome;

internal static class Extensions
{
    public static _HO.IncomeDataOther ToDomainServiceRequest(this Dto.IncomeDataOther request)
        => new()
        {
            IncomeOtherTypeId = request.IncomeOtherTypeId
        };

    public static _HO.IncomeDataEntrepreneur ToDomainServiceRequest(this Dto.IncomeDataEntrepreneur request)
        => new()
        {
            BirthNumber = request.BirthNumber ?? "",
            Cin = request.Cin ?? "",
            CountryOfResidenceId = request.CountryOfResidenceId
        };

    public static _HO.IncomeDataEmployement ToDomainServiceRequest(this Dto.IncomeDataEmployement request)
        => new()
        {
            ForeignIncomeTypeId = request.ForeignIncomeTypeId,
            Employer = request.Employer?.ToDomainServiceRequest(),
            HasProofOfIncome = request.HasProofOfIncome,
            HasWageDeduction = request.HasWageDeduction,
            IncomeConfirmation = request.IncomeConfirmation?.ToDomainServiceRequest(),
            Job = request.Job?.ToDomainServiceRequest(),
            WageDeduction = request.WageDeduction?.ToDomainServiceRequest()
        };

    public static _HO.EmployerData ToDomainServiceRequest(this Dto.EmployerDataDto contract)
        => new()
        {
            CountryId = contract.CountryId,
            BirthNumber = contract.BirthNumber ?? "",
            Cin = contract.Cin ?? "",
            Name = contract.Name ?? ""
        };

    public static _HO.IncomeConfirmationData ToDomainServiceRequest(this Dto.IncomeConfirmationDataDto contract)
        => new()
        {
            IsIssuedByExternalAccountant = contract.IsIssuedByExternalAccountant,
            ConfirmationContact = contract.ConfirmationContact ?? "",
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson ?? ""
        };

    public static _HO.JobData ToDomainServiceRequest(this Dto.JobDataDto contract)
        => new()
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription ?? "",
            IsInProbationaryPeriod = contract.IsInProbationaryPeriod,
            IsInTrialPeriod = contract.IsInTrialPeriod,
            GrossAnnualIncome = contract.GrossAnnualIncome
        };

    public static _HO.WageDeductionData ToDomainServiceRequest(this Dto.WageDeductionDataDto contract)
        => new()
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
