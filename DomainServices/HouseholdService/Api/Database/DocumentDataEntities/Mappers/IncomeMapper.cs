using CIS.Core.Attributes;
using DomainServices.CodebookService.Clients;
using __Contracts = DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;

namespace DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class IncomeMapper
{
    public async Task<Income> MapToData(
        int incomeTypeId,
        __Contracts.IncomeBaseData baseData,
        __Contracts.IncomeDataEmployement? dataEmployement,
        __Contracts.IncomeDataEntrepreneur? dataEntrepreneur,
        __Contracts.IncomeDataOther? dataOther,
        CancellationToken cancellationToken)
    {
        var model = new Income
        {
            Sum = baseData?.Sum,
            CurrencyCode = baseData?.CurrencyCode,
            IncomeTypeId = (CustomerIncomeTypes)incomeTypeId
        };

        model.HasProofOfIncome = (model.IncomeTypeId == CustomerIncomeTypes.Employement ? dataEmployement?.HasProofOfIncome : null) ?? false;
        model.IncomeSource = await getIncomeSource(model.IncomeTypeId, dataEmployement, dataOther, cancellationToken);

        switch (model.IncomeTypeId)
        {
            case CustomerIncomeTypes.Employement:
                model.Employement = mapEmployementToData(dataEmployement);
                break;

            case CustomerIncomeTypes.Entrepreneur:
                model.Entrepreneur = new()
                {
                    BirthNumber = dataEntrepreneur?.BirthNumber,
                    Cin = dataEntrepreneur?.Cin,
                    CountryOfResidenceId = dataEntrepreneur?.CountryOfResidenceId
                };
                break;

            case CustomerIncomeTypes.Other:
                model.Other = new()
                {
                    IncomeOtherTypeId = dataOther?.IncomeOtherTypeId
                };
                break;
        }

        return model;
    }

    public __Contracts.IncomeInList MapFromDataToList(DocumentDataItem<Income> item)
    {
        return new __Contracts.IncomeInList
        {
            IncomeId = item.DocumentDataStorageId,
            IncomeTypeId = (int)item.Data!.IncomeTypeId,
            CurrencyCode = item.Data.CurrencyCode,
            Sum = item.Data.Sum,
            IncomeSource = item.Data.IncomeSource,
            HasProofOfIncome = item.Data.HasProofOfIncome
        };
    }

    public __Contracts.Income MapFromDataToSingle(Income? data)
    {
        if (data is null) return new __Contracts.Income();

        var model = new __Contracts.Income
        {
            IncomeTypeId = (int)data.IncomeTypeId,
            BaseData = new __Contracts.IncomeBaseData
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
                model.Rent = new __Contracts.IncomeDataRent();
                break;

            default:
                throw new NotImplementedException("This customer income type deserializer is not implemented");
        }
        return model;
    }

    private static Income.IncomeEmployement mapEmployementToData(__Contracts.IncomeDataEmployement? source)
    {
        if (source == null) return new Income.IncomeEmployement();

        return new Income.IncomeEmployement
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

    private async Task<string?> getIncomeSource(
        CustomerIncomeTypes incomeTypeId,
        __Contracts.IncomeDataEmployement? dataEmployement,
        __Contracts.IncomeDataOther? dataOther,
        CancellationToken cancellationToken)
        => incomeTypeId switch
        {
            CustomerIncomeTypes.Employement => string.IsNullOrEmpty(dataEmployement?.Employer.Name) ? "-" : dataEmployement?.Employer.Name,
            CustomerIncomeTypes.Other => await getOtherIncomeName(dataOther?.IncomeOtherTypeId, cancellationToken),
            _ => "-"
        };

    private async Task<string?> getOtherIncomeName(int? id, CancellationToken cancellationToken)
        => id.HasValue ? (await _codebookService.IncomeOtherTypes(cancellationToken)).FirstOrDefault(t => t.Id == id)?.Name : "-";

    private static __Contracts.IncomeDataEmployement mapEmployementFromData(Income.IncomeEmployement? data)
    {
        if (data is null) return new __Contracts.IncomeDataEmployement();

        return new __Contracts.IncomeDataEmployement
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

    private readonly ICodebookServiceClient _codebookService;

    public IncomeMapper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}
