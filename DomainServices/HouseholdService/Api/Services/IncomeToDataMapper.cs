using CIS.Core.Attributes;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace DomainServices.HouseholdService.Api.Services;

[TransientService, SelfService]
internal sealed class IncomeToDataMapper
{
    public async Task<Database.DocumentDataEntities.Income> MapToDocumentData(
        int incomeTypeId,
        IncomeBaseData baseData, 
        IncomeDataEmployement? dataEmployement, 
        IncomeDataEntrepreneur? dataEntrepreneur, 
        IncomeDataOther? dataOther,
        CancellationToken cancellationToken)
    {
        var model = new Database.DocumentDataEntities.Income
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
                model.Employement = mapEmployementToData(dataEmployement!);
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

    private static Database.DocumentDataEntities.Income.IncomeEmployement mapEmployementToData(IncomeDataEmployement source)
    {
        return new Database.DocumentDataEntities.Income.IncomeEmployement
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
        IncomeDataEmployement? dataEmployement,
        IncomeDataOther? dataOther, 
        CancellationToken cancellationToken)
        => incomeTypeId switch
        {
            CustomerIncomeTypes.Employement => string.IsNullOrEmpty(dataEmployement?.Employer.Name) ? "-" : dataEmployement?.Employer.Name,
            CustomerIncomeTypes.Other => await getOtherIncomeName(dataOther?.IncomeOtherTypeId, cancellationToken),
            _ => "-"
        };

    private async Task<string?> getOtherIncomeName(int? id, CancellationToken cancellationToken)
        => id.HasValue ? (await _codebookService.IncomeOtherTypes(cancellationToken)).FirstOrDefault(t => t.Id == id)?.Name : "-";

    private readonly ICodebookServiceClient _codebookService;

    public IncomeToDataMapper(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }
}
