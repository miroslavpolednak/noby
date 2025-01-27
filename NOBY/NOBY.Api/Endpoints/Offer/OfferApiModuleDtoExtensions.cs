﻿using DomainServices.OfferService.Contracts;
using Google.Protobuf.Collections;

namespace NOBY.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static OfferMortgageInputsExtended ToApiResponse(this MortgageOfferSimulationInputs input, MortgageOfferBasicParameters basicParams)
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
            LoanPurposes = input.LoanPurposes?.Select(t => new SharedTypesLoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            Developer = input.Developer is null ? null : new OfferSharedDeveloper
            {
                DeveloperId = input.Developer.DeveloperId,
                Description = input.Developer.Description,
                ProjectId = input.Developer.ProjectId
            },
            DrawingDurationId = input.DrawingDurationId,
            DrawingTypeId = input.DrawingTypeId,
            InterestRateDiscount = input.InterestRateDiscount,
            MarketingActions = input.MarketingActions is null ? null : new OfferSharedMarketingActionInputItemResult
            {
                Domicile = input.MarketingActions.Domicile,
                HealthRiskInsurance = input.MarketingActions.HealthRiskInsurance,
                IncomeLoanRatioDiscount = input.MarketingActions.IncomeLoanRatioDiscount,
                RealEstateInsurance = input.MarketingActions.RealEstateInsurance,
                UserVip = input.MarketingActions.UserVip
            },
            Fees = input?.Fees.Select(f => new OfferSharedFeeInputItem
            {
                DiscountPercentage = f.DiscountPercentage,
                FeeId = f.FeeId
            }).ToList(),
            RiskLifeInsurance = input?.RiskLifeInsurance is null ? null : new OfferSharedInsuranceItem
            {
                Sum = input.RiskLifeInsurance.Sum,
                Frequency = input.RiskLifeInsurance.Frequency
            },
            RealEstateInsurance = input?.RealEstateInsurance is null ? null : new OfferSharedInsuranceItem
            {
                Sum = input.RealEstateInsurance.Sum,
                Frequency = input.RealEstateInsurance.Frequency
            }
        };
    
    public static OfferSharedMortgageOutputs ToApiResponse(this MortgageOfferSimulationResults result, MortgageOfferSimulationInputs inputs, MortgageOfferAdditionalSimulationResults additionalResults)
        => new()
        {
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            LoanDuration = result.LoanDuration,
            LoanTotalAmount = result.LoanTotalAmount,
            LoanToValue = result.LoanToValue,
            LoanAmount = result.LoanAmount,
            LoanPaymentAmount = result.LoanPaymentAmount,
            LoanPurposes = inputs.LoanPurposes?.Select(t => new SharedTypesLoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
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
            MarketingActionsDeviation = result.MarketingActionsDeviation,
            MarketingActions = additionalResults.MarketingActions?.Select(i => i.ToApiResponseItem()).ToList(),
            PaymentScheduleSimple = additionalResults.PaymentScheduleSimple?.Select(p => p.ToApiResponseItem()).ToList(),
            Fees = additionalResults.Fees is null ? null : additionalResults.Fees.ToApiResponse(),
            Warnings = result.Warnings?.Select(t => t.ToApiResponseItem())?.ToList()
        };

    public static OfferSharedMortgageOutputsOutputWarning ToApiResponseItem(this SimulationResultWarning resultItem)
        => new()
        {
            InternalMessage = resultItem.WarningInternalMessage,
            Text = resultItem.WarningText
        };

    public static OfferSharedMortgageOutputsPaymentScheduleSimpleItem ToApiResponseItem(this PaymentScheduleSimple resultItem)
        => new()
        {
            Amount = resultItem.Amount,
            Date = resultItem.Date,
            PaymentNumber = resultItem.PaymentNumber,
            Type = resultItem.Type,
        };

    public static OfferSharedMortgageOutputsMarketingActionItem ToApiResponseItem(this ResultMarketingAction resultItem)
        => new()
        {
            Code = resultItem.Code,
            Requested = resultItem.Requested == 1,
            Applied = resultItem.Applied == 1,
            MarketingActionId = resultItem.MarketingActionId,
            Deviation = resultItem.Deviation,
            Name = resultItem.Name
        };

    public static List<OfferSharedMortgageOutputsFeeItem> ToApiResponse(this RepeatedField<ResultFee> fees)
        => fees.Select(t => new OfferSharedMortgageOutputsFeeItem
        {
            FeeId = t.FeeId,
            DiscountPercentage = t.DiscountPercentage,
            DisplayAsFreeOfCharge = t.DisplayAsFreeOfCharge,
            ShortNameForExample = t.ShortNameForExample,
            AccountDateFrom = t.AccountDateFrom,
            CodeKB = t.CodeKB,
            ComposedSum = t.ComposedSum,
            FinalSum = t.FinalSum,
            IncludeInRPSN = t.IncludeInRPSN,
            TariffSum = t.TariffSum,
            MarketingActionId = t.MarketingActionId,
            Name = t.Name,
            Periodicity = t.Periodicity,
            TariffName = t.TariffName,
            TariffTextWithAmount = t.TariffTextWithAmount,
            UsageText = t.UsageText
        }).ToList();

    public static OfferSimulateMortgageCreditWorthinessSimpleResults? ToApiResponse(this MortgageOfferCreditWorthinessSimpleResults? result)
    {
        if (result is null)
            return null;

        return new()
        {
            WorthinessResult = (OfferSimulateMortgageCreditWorthinessSimpleResultsWorthinessResult)result.WorthinessResult,
            InstallmentLimit = result.InstallmentLimit ?? 0,
            MaxAmount = result.MaxAmount ?? 0,
            RemainsLivingAnnuity = result.RemainsLivingAnnuity ?? 0,
            RemainsLivingInst = result.RemainsLivingInst ?? 0,
        };
    }

    public static OfferSharedCreditWorthinessSimpleInputs? ToApiResponse(this MortgageOfferCreditWorthinessSimpleInputs? inputs, bool isActive)
    {
        if (inputs is null)
            return null;

        return new()
        {
            IsActive = isActive,
            TotalMonthlyIncome = inputs.TotalMonthlyIncome,
            ExpensesRent = inputs.ExpensesSummary?.Rent,
            ExpensesOther = inputs.ExpensesSummary?.Other,
            LoansInstallmentsAmount = inputs.ObligationsSummary?.LoansInstallmentsAmount,
            CreditCardsAmount = inputs.ObligationsSummary?.CreditCardsAmount,
            AuthorizedOverdraftsTotalAmount = inputs.ObligationsSummary?.AuthorizedOverdraftsTotalAmount,
            ChildrenCount = inputs.ChildrenCount
        };
    }
}