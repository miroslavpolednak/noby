using DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static Dto.MortgageInputs ToApiResponse(this SimulationInputs input, BasicParameters basicParams)
        => new()
        {
            ProductTypeId = input.ProductTypeId,
            LoanKindId = input.LoanKindId,
            LoanAmount = input.LoanAmount,
            LoanDuration = input.LoanDuration,
            CollateralAmount = input.CollateralAmount,
            PaymentDay = input.PaymentDay,
            IsEmployeeBonusRequested = input.IsEmployeeBonusRequested,
            FixedRatePeriod = input.FixedRatePeriod!.Value,
            ExpectedDateOfDrawing = input.ExpectedDateOfDrawing,
            FinancialResourcesOwn = basicParams.FinancialResourcesOwn,
            FinancialResourcesOther = basicParams.FinancialResourcesOther,
            GuaranteeDateFrom = (DateTime)input.GuaranteeDateFrom!,
            LoanPurposes = input.LoanPurposes?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
        };
    
    public static Dto.MortgageOutputs ToApiResponse(this BaseSimulationResults result, SimulationInputs inputs)
        => new()
        {
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            LoanDuration = result.LoanDuration,
            LoanTotalAmount = result.LoanTotalAmount,
            LoanToValue = result.LoanToValue,
            LoanAmount = result.LoanAmount,
            LoanPaymentAmount = result.LoanPaymentAmount,
            LoanPurposes = inputs.LoanPurposes?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            PaymentDay = inputs.PaymentDay,
            LoanDueDate = result.LoanDueDate,
            LoanInterestRateProvided = result.LoanInterestRateProvided,
            ContractSignedDate = result.ContractSignedDate,
            DrawingDateTo = result.DrawingDateTo,
            AnnuityPaymentsDateFrom = result.AnnuityPaymentsDateFrom,
            AnnuityPaymentsCount = result.AnnuityPaymentsCount,
            LoanInterestRate = result.LoanInterestRate,
            LoanInterestRateAnnounced = result.LoanInterestRateAnnounced,
            LoanInterestRateAnnouncedType = result.LoanInterestRateAnnouncedType,
            EmployeeBonusDeviation = result.EmployeeBonusDeviation,
            MarketingActionsDeviation = result.MarketingActionsDeviation
        };
}