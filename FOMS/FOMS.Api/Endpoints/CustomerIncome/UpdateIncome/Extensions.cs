using _SA = DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

internal static class Extensions
{
    public static _SA.IncomeDataEmployement ToDomainServiceRequest(this Dto.IncomeDataEmployement request)
    {
        return new _SA.IncomeDataEmployement
        {
            AbroadIncomeTypeId = request.AbroadIncomeTypeId,
            IsDomicile = request.IsDomicile,
            IsAbroadIncome = request.IsAbroadIncome,
            Employer = request.Employer?.ToDomainServiceRequest(),
            IncomeConfirmation = request.IncomeConfirmation?.ToDomainServiceRequest(),
            Job = request.Job?.ToDomainServiceRequest(),
            WageDeduction = request.WageDeduction?.ToDomainServiceRequest()
        };
    }

    public static _SA.EmployerData ToDomainServiceRequest(this Dto.EmployerDataDto contract)
        => new _SA.EmployerData
        {
            Address = contract.Address ?? new CIS.Foms.Types.Address(),
            SensitiveSector = contract.SensitiveSector,
            WorkSectorId = contract.WorkSectorId,
            BirthNumber = contract.BirthNumber ?? "",
            Cin = contract.Cin ?? "",
            ClassificationOfEconomicActivitiesId = contract.ClassificationOfEconomicActivitiesId,
            Name = contract.Name ?? "",
            PhoneNumber = contract.PhoneNumber ?? ""
        };

    public static _SA.IncomeConfirmationData ToDomainServiceRequest(this Dto.IncomeConfirmationDataDto contract)
        => new _SA.IncomeConfirmationData
        {
            ConfirmationByCompany = contract.ConfirmationByCompany,
            ConfirmationContact = contract.ConfirmationContact ?? "",
            ConfirmationDate = contract.ConfirmationDate,
            ConfirmationPerson = contract.ConfirmationPerson ?? "",
            ConfirmationPlace = contract.ConfirmationPlace ?? ""
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
            JobType = contract.JobType
        };

    public static _SA.WageDeductionData ToDomainServiceRequest(this Dto.WageDeductionDataDto contract)
        => new _SA.WageDeductionData
        {
            DeductionDecision = contract.DeductionDecision,
            DeductionOther = contract.DeductionOther,
            DeductionPayments = contract.DeductionPayments
        };
}
