﻿using DomainServices.OfferService.Contracts;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.Offer.Dto;
using NOBY.Api.Endpoints.Offer.SimulateMortgage;

namespace NOBY.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static Dto.MortgageInputsExtended ToApiResponse(this MortgageSimulationInputs input, BasicParameters basicParams)
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
            LoanPurposes = input.LoanPurposes?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            Developer = input.Developer is null ? null : new Dto.Developer
            {
                DeveloperId = input.Developer.DeveloperId,
                Description = input.Developer.Description,
                ProjectId = input.Developer.ProjectId
            },
            DrawingDurationId = input.DrawingDurationId,
            DrawingTypeId = input.DrawingTypeId,
            InterestRateDiscount = input.InterestRateDiscount,
            MarketingActions = input.MarketingActions is null ? null : new Dto.MarketingActionInputItemResult
            {
                Domicile = input.MarketingActions.Domicile,
                HealthRiskInsurance = input.MarketingActions.HealthRiskInsurance,
                IncomeLoanRatioDiscount = input.MarketingActions.IncomeLoanRatioDiscount,
                RealEstateInsurance = input.MarketingActions.RealEstateInsurance,
                UserVip = input.MarketingActions.UserVip
            },
            Fees = input.Fees is null ? null : input.Fees.Select(f => new Dto.FeeInputItem
            {
                DiscountPercentage = f.DiscountPercentage,
                FeeId = f.FeeId
            }).ToList(),
            RiskLifeInsurance = input.RiskLifeInsurance is null ? null : new Dto.InsuranceItem
            {
                Sum = input.RiskLifeInsurance.Sum,
                Frequency = input.RiskLifeInsurance.Frequency
            },
            RealEstateInsurance = input.RealEstateInsurance is null ? null : new Dto.InsuranceItem
            {
                Sum = input.RealEstateInsurance.Sum,
                Frequency = input.RealEstateInsurance.Frequency
            }
        };
    
    public static Dto.MortgageOutputs ToApiResponse(this MortgageSimulationResults result, MortgageSimulationInputs inputs, AdditionalMortgageSimulationResults additionalResults)
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
            MarketingActionsDeviation = result.MarketingActionsDeviation,
            MarketingActions = additionalResults.MarketingActions?.Select(i => i.ToApiResponseItem()).ToList(),
            PaymentScheduleSimple = additionalResults.PaymentScheduleSimple?.Select(p => p.ToApiResponseItem()).ToList(),
            Fees = additionalResults.Fees is null ? null : additionalResults.Fees.ToApiResponse(),
            Warnings = result.Warnings?.Select(t => t.ToApiResponseItem())?.ToList()
        };

    public static Dto.OutputWarning ToApiResponseItem(this SimulationResultWarning resultItem)
        => new Dto.OutputWarning()
        {
            InternalMessage = resultItem.WarningInternalMessage,
            Text = resultItem.WarningText
        };

    public static Dto.PaymentScheduleSimpleItem ToApiResponseItem(this PaymentScheduleSimple resultItem)
        => new Dto.PaymentScheduleSimpleItem()
        {
            Amount = resultItem.Amount,
            Date = resultItem.Date,
            PaymentNumber = resultItem.PaymentNumber,
            Type = resultItem.Type,
        };

    public static Dto.MarketingActionItem ToApiResponseItem(this ResultMarketingAction resultItem)
        => new Dto.MarketingActionItem()
        {
            Code = resultItem.Code,
            Requested = resultItem.Requested == 1,
            Applied = resultItem.Applied == 1,
            MarketingActionId = resultItem.MarketingActionId,
            Deviation = resultItem.Deviation,
            Name = resultItem.Name
        };

    public static List<Dto.FeeItem> ToApiResponse(this RepeatedField<ResultFee> fees)
        => fees.Select(t => new Dto.FeeItem
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

    public static CreditWorthinessSimpleResults? ToApiResponse(this MortgageCreditWorthinessSimpleResults? result)
    {
        if (result is null)
            return null;

        return new CreditWorthinessSimpleResults
        {
            WorthinessResult = (SimulateMortgage.WorthinessResult)result.WorthinessResult,
            InstallmentLimit = result.InstallmentLimit ?? 0,
            MaxAmount = result.MaxAmount ?? 0,
            RemainsLivingAnnuity = result.RemainsLivingAnnuity ?? 0,
            RemainsLivingInst = result.RemainsLivingInst ?? 0,
        };
    }

    public static CreditWorthinessSimpleInputs? ToApiResponse(this MortgageCreditWorthinessSimpleInputs? inputs, bool isActive)
    {
        if (inputs is null)
            return null;

        return new CreditWorthinessSimpleInputs
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