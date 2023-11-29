using CIS.Core.Attributes;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Services;

[TransientService, SelfService]
internal sealed class IncomeFromDataMapper
{
    public IncomeInList MapDataToList(DocumentDataItem<Database.DocumentDataEntities.Income> item)
    {
        return new IncomeInList
        {
            IncomeId = item.DocumentDataStorageId,
            IncomeTypeId = (int)item.Data!.IncomeTypeId,
            CurrencyCode = item.Data.CurrencyCode,
            Sum = item.Data.Sum,
            IncomeSource = item.Data.IncomeSource,
            HasProofOfIncome = item.Data.HasProofOfIncome
        };
    }

#pragma warning disable CA1822 // Mark members as static
    public Income MapDataToSingle(Database.DocumentDataEntities.Income data)
#pragma warning restore CA1822 // Mark members as static
    {
        var model = new Income
        {
            IncomeTypeId = (int)data.IncomeTypeId,
            BaseData = new IncomeBaseData
            {
                CurrencyCode = data.CurrencyCode,
                Sum = data.Sum
            }
        };

        switch (data.IncomeTypeId)
        {
            case CustomerIncomeTypes.Employement:
                model.Employement = mapEmployementFromData(data.Employement!);
                break;

            case CustomerIncomeTypes.Other:
                model.Other = new()
                {
                    IncomeOtherTypeId = data.Other?.IncomeOtherTypeId
                };
                break;

            case CustomerIncomeTypes.Entrepreneur:
                model.Entrepreneur = new()
                {
                    BirthNumber = data.Entrepreneur?.BirthNumber,
                    Cin = data.Entrepreneur?.Cin,
                    CountryOfResidenceId = data.Entrepreneur?.CountryOfResidenceId
                };
                break;

            case CustomerIncomeTypes.Rent:
                model.Rent = new IncomeDataRent();
                break;

            default:
                throw new NotImplementedException("This customer income type deserializer is not implemented");
        }
        return model;
    }

    private static IncomeDataEmployement mapEmployementFromData(Database.DocumentDataEntities.Income.IncomeEmployement data)
    {
        return new IncomeDataEmployement
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
