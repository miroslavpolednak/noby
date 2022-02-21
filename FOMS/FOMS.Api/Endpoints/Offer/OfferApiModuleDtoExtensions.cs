﻿namespace FOMS.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static Dto.MortgageInputs ToResponseDto(this DomainServices.OfferService.Contracts.MortgageInput result)
        => new()
        {
            ProductTypeId = result.ProductTypeId,
            ProductLoanKindId = result.LoanKindId,
            LoanAmount = result.LoanAmount,
            LoanDuration = result.LoanDuration,
            LoanPaymentAmount = result.LoanPaymentAmount,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            CollateralAmount = result.CollateralAmount,
            LoanToValue = result.LoanToValue,
            PaymentDayOfTheMonth = result.PaymentDayOfTheMonth,
            EmployeeBonusRequested = result.EmployeeBonusRequested
        };
    
    public static Dto.MortgageOutputs ToResponseDto(this DomainServices.OfferService.Contracts.MortgageOutput result)
        => new()
        {
            StatementTypeId = result.StatementTypeId,
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            InterestRate = result.InterestRate,
            InterestRateAnnounced = result.InterestRateAnnounced,
            LoanDuration = result.LoanDuration,
            LoanToCost = result.LoanToCost,
            LoanTotalAmount = result.LoanTotalAmount,
            LoanToValue = result.LoanToValue,
            LoanAmount = result.LoanAmount,
            LoanPaymentAmount = result.LoanPaymentAmount,
            LoanPurpose = result.LoanPurpose?.Select(t => t.ProductLoanPurposeId).ToList() ?? default(List<int>)
        };
}