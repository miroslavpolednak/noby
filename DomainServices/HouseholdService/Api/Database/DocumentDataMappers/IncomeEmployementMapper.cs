using __Data = DomainServices.HouseholdService.Api.Database.DocumentDataEntities;
using __Contract = DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataMappers;

internal class IncomeEmployementMapper
    : IDocumentDataMapper<__Data.IncomeEmployement, __Contract.IncomeDataEmployement>
{
    public __Data.IncomeEmployement MapToDocumentData(__Contract.IncomeDataEmployement source)
    {
        return new __Data.IncomeEmployement
        {
            ForeignIncomeTypeId = source.ForeignIncomeTypeId,
            HasWageDeduction = source.HasWageDeduction,
            HasProofOfIncome = source.HasProofOfIncome,
            Employer = new()
            {
                BirthNumber = source.Employer.BirthNumber,
                Cin = source.Employer.Cin,
                CountryId = source.Employer.CountryId,
                Name = source.Employer.Name
            },
            IncomeConfirmation = source.IncomeConfirmation is null ? null : new()
            {
                ConfirmationDate = source.IncomeConfirmation.ConfirmationDate,
                ConfirmationPerson = source.IncomeConfirmation.ConfirmationPerson,
                IsIssuedByExternalAccountant = source.IncomeConfirmation.IsIssuedByExternalAccountant,
                ConfirmationContact = source.IncomeConfirmation.ConfirmationContact is null ? null : new()
                {
                    PhoneIDC = source.IncomeConfirmation.ConfirmationContact.PhoneIDC,
                    PhoneNumber = source.IncomeConfirmation.ConfirmationContact.PhoneNumber
                },
            },
            Job = source.Job is null ? null : new()
            {
                CurrentWorkContractSince = source.Job.CurrentWorkContractSince,
                FirstWorkContractSince = source.Job.FirstWorkContractSince,
                CurrentWorkContractTo = source.Job.CurrentWorkContractTo,
                EmploymentTypeId = source.Job.EmploymentTypeId,
                GrossAnnualIncome = source.Job.GrossAnnualIncome,
                IsInProbationaryPeriod = source.Job.IsInProbationaryPeriod,
                IsInTrialPeriod = source.Job.IsInTrialPeriod,
                JobDescription = source.Job.JobDescription
            },
            WageDeduction = source.WageDeduction is null ? null : new()
            {
                DeductionDecision = source.WageDeduction.DeductionDecision,
                DeductionOther = source.WageDeduction.DeductionOther,
                DeductionPayments = source.WageDeduction.DeductionPayments
            }
        };
    }

    public __Contract.IncomeDataEmployement MapFromDocumentData(__Data.IncomeEmployement data)
    {
        return new __Contract.IncomeDataEmployement
        {
            ForeignIncomeTypeId = data.ForeignIncomeTypeId,
            HasWageDeduction = data.HasWageDeduction,
            HasProofOfIncome = data.HasProofOfIncome,
            Employer = new()
            {
                BirthNumber = data.Employer?.BirthNumber,
                Cin = data.Employer?.Cin,
                CountryId = data.Employer?.CountryId,
                Name = data.Employer?.Name
            },
            IncomeConfirmation = data.IncomeConfirmation is null ? null : new()
            {
                ConfirmationDate = data.IncomeConfirmation.ConfirmationDate,
                ConfirmationPerson = data.IncomeConfirmation.ConfirmationPerson,
                IsIssuedByExternalAccountant = data.IncomeConfirmation.IsIssuedByExternalAccountant,
                ConfirmationContact = data.IncomeConfirmation.ConfirmationContact is null ? null : new()
                {
                    PhoneIDC = data.IncomeConfirmation.ConfirmationContact.PhoneIDC,
                    PhoneNumber = data.IncomeConfirmation.ConfirmationContact.PhoneNumber
                }
            },
            Job = data.Job is null ? null : new()
            {
                CurrentWorkContractSince = data.Job.CurrentWorkContractSince,
                FirstWorkContractSince = data.Job.FirstWorkContractSince,
                CurrentWorkContractTo = data.Job.CurrentWorkContractTo,
                EmploymentTypeId = data.Job.EmploymentTypeId,
                GrossAnnualIncome = data.Job.GrossAnnualIncome,
                IsInProbationaryPeriod = data.Job.IsInProbationaryPeriod,
                IsInTrialPeriod = data.Job.IsInTrialPeriod,
                JobDescription = data.Job.JobDescription
            },
            WageDeduction = data.WageDeduction is null ? null : new()
            {
                DeductionDecision = data.WageDeduction.DeductionDecision,
                DeductionOther = data.WageDeduction.DeductionOther,
                DeductionPayments = data.WageDeduction.DeductionPayments
            }
        };
    }
}
