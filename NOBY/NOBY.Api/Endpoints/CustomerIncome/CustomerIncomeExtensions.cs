using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome;

internal static class CustomerIncomeExtensions
{
    public static CustomerIncomeSharedDataOther ToApiResponse(this _HO.IncomeDataOther contract)
        => new()
        {
            IncomeOtherTypeId = contract.IncomeOtherTypeId
        };

    public static CustomerIncomeSharedDataEntrepreneur ToApiResponse(this _HO.IncomeDataEntrepreneur contract)
        => new()
        {
            BirthNumber = contract.BirthNumber,
            Cin = contract.Cin,
            CountryOfResidenceId = contract.CountryOfResidenceId
        };

    public static CustomerIncomeSharedDataEmployement ToApiResponse(this _HO.IncomeDataEmployement contract)
        => new()
        {
            ForeignIncomeTypeId = contract.ForeignIncomeTypeId,
            HasProofOfIncome = contract.HasProofOfIncome,
            HasWageDeduction = contract.HasWageDeduction,
            Employer = contract.Employer is null ? null : new()
            {
                BirthNumber = contract.Employer.BirthNumber,
                Cin = contract.Employer.Cin,
                Name = contract.Employer.Name,
                CountryId = contract.Employer.CountryId
            },
            IncomeConfirmation = contract.IncomeConfirmation is null ? null : new()
            {
                IsIssuedByExternalAccountant = contract.IncomeConfirmation.IsIssuedByExternalAccountant,
                ConfirmationContact = contract.IncomeConfirmation.ConfirmationContact is null ? null : new()
                {
                    PhoneIDC = contract.IncomeConfirmation.ConfirmationContact.PhoneIDC,
                    PhoneNumber = contract.IncomeConfirmation.ConfirmationContact.PhoneNumber
                },
                ConfirmationDate = contract.IncomeConfirmation.ConfirmationDate,
                ConfirmationPerson = contract.IncomeConfirmation.ConfirmationPerson
            },
            Job = contract.Job is null ? null :  new()
            {
                CurrentWorkContractSince = contract.Job.CurrentWorkContractSince,
                FirstWorkContractSince = contract.Job.FirstWorkContractSince,
                CurrentWorkContractTo = contract.Job.CurrentWorkContractTo,
                EmploymentTypeId = contract.Job.EmploymentTypeId,
                JobDescription = contract.Job.JobDescription,
                IsInProbationaryPeriod = contract.Job.IsInProbationaryPeriod,
                IsInTrialPeriod = contract.Job.IsInTrialPeriod
            },
            WageDeduction = contract.WageDeduction is null ? null : new()
            {
                DeductionDecision = contract.WageDeduction.DeductionDecision,
                DeductionOther = contract.WageDeduction.DeductionOther,
                DeductionPayments = contract.WageDeduction.DeductionPayments
            }
        };

    public static _HO.IncomeDataOther ToDomainServiceRequest(this CustomerIncomeSharedDataOther request)
        => new()
        {
            IncomeOtherTypeId = request.IncomeOtherTypeId
        };

    public static _HO.IncomeDataEntrepreneur ToDomainServiceRequest(this CustomerIncomeSharedDataEntrepreneur request)
        => new()
        {
            BirthNumber = request.BirthNumber ?? "",
            Cin = request.Cin ?? "",
            CountryOfResidenceId = request.CountryOfResidenceId
        };

    public static _HO.IncomeDataEmployement ToDomainServiceRequest(this CustomerIncomeSharedDataEmployement request)
        => new()
        {
            ForeignIncomeTypeId = request.ForeignIncomeTypeId,
            Employer = request.Employer is null ? null : new()
            {
                CountryId = request.Employer.CountryId,
                BirthNumber = request.Employer.BirthNumber ?? "",
                Cin = request.Employer.Cin ?? "",
                Name = request.Employer.Name ?? ""
            },
            HasProofOfIncome = request.HasProofOfIncome,
            HasWageDeduction = request.HasWageDeduction,
            IncomeConfirmation = request.IncomeConfirmation is null ? null : new()
            {
                IsIssuedByExternalAccountant = request.IncomeConfirmation.IsIssuedByExternalAccountant,
                ConfirmationContact = request.IncomeConfirmation.ConfirmationContact is null ? null : new()
                {
                    PhoneIDC = request.IncomeConfirmation.ConfirmationContact.PhoneIDC,
                    PhoneNumber = request.IncomeConfirmation.ConfirmationContact.PhoneNumber
                },
                ConfirmationDate = request.IncomeConfirmation.ConfirmationDate,
                ConfirmationPerson = request.IncomeConfirmation.ConfirmationPerson ?? ""
            },
            Job = request.Job is null ? null : new()
            {
                CurrentWorkContractSince = request.Job.CurrentWorkContractSince,
                FirstWorkContractSince = request.Job.FirstWorkContractSince,
                CurrentWorkContractTo = request.Job.CurrentWorkContractTo,
                EmploymentTypeId = request.Job.EmploymentTypeId,
                JobDescription = request.Job.JobDescription ?? "",
                IsInProbationaryPeriod = request.Job.IsInProbationaryPeriod,
                IsInTrialPeriod = request.Job.IsInTrialPeriod
            },
            WageDeduction = request.WageDeduction is null ? null : new()
            {
                DeductionDecision = request.WageDeduction.DeductionDecision,
                DeductionOther = request.WageDeduction.DeductionOther,
                DeductionPayments = request.WageDeduction.DeductionPayments
            }
        };
}
