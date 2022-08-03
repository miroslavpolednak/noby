using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome;

internal static class Extensions
{
    public static _SA.IncomeDataOther ToDomainServiceRequest(this Dto.IncomeDataOther request)
        => new _SA.IncomeDataOther
        {
            IncomeOtherTypeId = request.IncomeOtherTypeId
        };

    public static _SA.IncomeDataEntrepreneur ToDomainServiceRequest(this Dto.IncomeDataEntrepreneur request)
        => new _SA.IncomeDataEntrepreneur
        {
            BirthNumber = request.BirthNumber,
            Cin = request.Cin,
            CountryOfResidenceId = request.CountryOfResidenceId
        };

    public static _SA.IncomeDataEmployement ToDomainServiceRequest(this Dto.IncomeDataEmployement request)
        => new _SA.IncomeDataEmployement
        {
            ForeignIncomeTypeId = request.ForeignIncomeTypeId,
            Employer = request.Employer?.ToDomainServiceRequest(),
            ProofOfIncomeToggle = request.ProofOfIncomeToggle,
            WageDeductionToggle = request.WageDeductionToggle,
            IncomeConfirmation = request.IncomeConfirmation?.ToDomainServiceRequest(),
            Job = request.Job?.ToDomainServiceRequest(),
            WageDeduction = request.WageDeduction?.ToDomainServiceRequest()
        };

    public static _SA.EmployerData ToDomainServiceRequest(this Dto.EmployerDataDto contract)
        => new _SA.EmployerData
        {
            CountryId = contract.CountryId,
            BirthNumber = contract.BirthNumber ?? "",
            Cin = contract.Cin ?? "",
            Name = contract.Name ?? ""
        };

    public static _SA.IncomeConfirmationData ToDomainServiceRequest(this Dto.IncomeConfirmationDataDto contract)
        => new _SA.IncomeConfirmationData
        {
            ConfirmationByCompany = contract.ConfirmationByCompany,
            ConfirmationContact = contract.ConfirmationContact ?? "",
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson ?? ""
        };

    public static _SA.JobData ToDomainServiceRequest(this Dto.JobDataDto contract)
        => new _SA.JobData
        {
            CurrentWorkContractSince = contract.CurrentWorkContractSince,
            FirstWorkContractSince = contract.FirstWorkContractSince,
            CurrentWorkContractTo = contract.CurrentWorkContractTo,
            EmploymentTypeId = contract.EmploymentTypeId,
            JobDescription = contract.JobDescription ?? "",
            JobNoticePeriod = contract.JobNoticePeriod,
            JobTrialPeriod = contract.JobTrialPeriod,
            GrossAnnualIncome = contract.GrossAnnualIncome
        };

    public static _SA.WageDeductionData ToDomainServiceRequest(this Dto.WageDeductionDataDto contract)
        => new _SA.WageDeductionData
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
