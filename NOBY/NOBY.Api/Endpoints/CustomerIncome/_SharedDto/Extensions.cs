using _HO = DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.CustomerIncome;

internal static class Extensions
{
    public static _HO.IncomeDataOther ToDomainServiceRequest(this SharedDto.IncomeDataOther request)
        => new()
        {
            IncomeOtherTypeId = request.IncomeOtherTypeId
        };

    public static _HO.IncomeDataEntrepreneur ToDomainServiceRequest(this SharedDto.IncomeDataEntrepreneur request)
        => new()
        {
            BirthNumber = request.BirthNumber ?? "",
            Cin = request.Cin ?? "",
            CountryOfResidenceId = request.CountryOfResidenceId
        };

    public static _HO.IncomeDataEmployement ToDomainServiceRequest(this SharedDto.IncomeDataEmployement request)
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

    public static _HO.EmployerData ToDomainServiceRequest(this SharedDto.EmployerDataDto contract)
        => new()
        {
            CountryId = contract.CountryId,
            BirthNumber = contract.BirthNumber ?? "",
            Cin = contract.Cin ?? "",
            Name = contract.Name ?? ""
        };

    public static _HO.IncomeConfirmationData ToDomainServiceRequest(this SharedDto.IncomeConfirmationDataDto contract)
        => new()
        {
            IsIssuedByExternalAccountant = contract.IsIssuedByExternalAccountant,
            ConfirmationContact = contract.ConfirmationContact is null ? null : new()
            {
                PhoneIDC = contract.ConfirmationContact.PhoneIDC,
                PhoneNumber = contract.ConfirmationContact.PhoneNumber
            },
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson ?? ""
        };

    public static _HO.JobData ToDomainServiceRequest(this SharedDto.JobDataDto contract)
        => new()
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription ?? "",
            IsInProbationaryPeriod = contract.IsInProbationaryPeriod,
            IsInTrialPeriod = contract.IsInTrialPeriod
        };

    public static _HO.WageDeductionData ToDomainServiceRequest(this SharedDto.WageDeductionDataDto contract)
        => new()
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
