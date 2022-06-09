using DomainServices.OfferService.Contracts;

namespace FOMS.Api.Endpoints.Offer.SimulateMortgage;

internal static class Extensions
{
    public static DomainServices.OfferService.Contracts.SimulateMortgageRequest ToDomainServiceRequest(this SimulateMortgageRequest request)
    {
        var model = new DomainServices.OfferService.Contracts.SimulateMortgageRequest()
        {
            ResourceProcessId = request.ResourceProcessId,
            SimulationInputs = new()
            {
                GuaranteeDateFrom = request.GuaranteeDateFrom,
                InterestRateDiscount = request.InterestRateDiscount,
                DrawingType = request.DrawingType,
                DrawingDuration = request.DrawingDuration,
                ProductTypeId = request.ProductTypeId,
                LoanKindId = request.LoanKindId,
                LoanAmount = request.LoanAmount,
                LoanDuration = request.LoanDuration.GetValueOrDefault(),
                FixedRatePeriod = request.FixedRatePeriod,
                CollateralAmount = request.CollateralAmount,
                PaymentDay = request.PaymentDay,
                IsEmployeeBonusRequested = request.IsEmployeeBonusRequested,
                ExpectedDateOfDrawing = request.ExpectedDateOfDrawing
            },
            BasicParameters = new()
            {
                FinancialResourcesOwn = request.FinancialResourcesOwn,
                FinancialResourcesOther = request.FinancialResourcesOther,
            }
        };
        
        if (request.LoanPurposes is not null && request.LoanPurposes.Any())
            model.SimulationInputs.LoanPurposes.AddRange(
                request.LoanPurposes.Select(t => new DomainServices.OfferService.Contracts.LoanPurpose() { LoanPurposeId = t.Id, Sum = t.Sum })
                );

        return model;
    }

    public static Dto.MortgageOutputs ToApiResponse(this SimulationResults result, SimulationInputs inputs)
        => new()
        {
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            InterestRate = result.LoanInterestRate,
            InterestRateAnnounced = result.LoanInterestRateAnnounced,
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