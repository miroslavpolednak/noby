using DomainServices.OfferService.Contracts;
using Google.Protobuf.Collections;
using NOBY.Api.Endpoints.Offer.SharedDto;
using NOBY.Api.Endpoints.Offer.SimulateMortgage;

namespace NOBY.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static SharedDto.MortgageInputsExtended ToApiResponse(this MortgageOfferSimulationInputs input, MortgageOfferBasicParameters basicParams)
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
            LoanPurposes = input.LoanPurposes?.Select(t => new SharedDto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            Developer = input.Developer is null ? null : new SharedDto.Developer
            {
                DeveloperId = input.Developer.DeveloperId,
                Description = input.Developer.Description,
                ProjectId = input.Developer.ProjectId
            },
            DrawingDurationId = input.DrawingDurationId,
            DrawingTypeId = input.DrawingTypeId,
            InterestRateDiscount = input.InterestRateDiscount,
            MarketingActions = input.MarketingActions is null ? null : new SharedDto.MarketingActionInputItemResult
            {
                Domicile = input.MarketingActions.Domicile,
                HealthRiskInsurance = input.MarketingActions.HealthRiskInsurance,
                IncomeLoanRatioDiscount = input.MarketingActions.IncomeLoanRatioDiscount,
                RealEstateInsurance = input.MarketingActions.RealEstateInsurance,
                UserVip = input.MarketingActions.UserVip
            },
            Fees = input.Fees is null ? null : input.Fees.Select(f => new SharedDto.FeeInputItem
            {
                DiscountPercentage = f.DiscountPercentage,
                FeeId = f.FeeId
            }).ToList(),
            RiskLifeInsurance = input.RiskLifeInsurance is null ? null : new SharedDto.InsuranceItem
            {
                Sum = input.RiskLifeInsurance.Sum,
                Frequency = input.RiskLifeInsurance.Frequency
            },
            RealEstateInsurance = input.RealEstateInsurance is null ? null : new SharedDto.InsuranceItem
            {
                Sum = input.RealEstateInsurance.Sum,
                Frequency = input.RealEstateInsurance.Frequency
            }
        };
    
    public static SharedDto.MortgageOutputs ToApiResponse(this MortgageOfferSimulationResults result, MortgageOfferSimulationInputs inputs, MortgageOfferAdditionalSimulationResults additionalResults)
        => new()
        {
            Aprc = result.Aprc,
            EmployeeBonusLoanCode = result.EmployeeBonusLoanCode,
            LoanDuration = result.LoanDuration,
            LoanTotalAmount = result.LoanTotalAmount,
            LoanToValue = result.LoanToValue,
            LoanAmount = result.LoanAmount,
            LoanPaymentAmount = result.LoanPaymentAmount,
            LoanPurposes = inputs.LoanPurposes?.Select(t => new SharedDto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
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

    public static SharedDto.OutputWarning ToApiResponseItem(this SimulationResultWarning resultItem)
        => new SharedDto.OutputWarning()
        {
            InternalMessage = resultItem.WarningInternalMessage,
            Text = resultItem.WarningText
        };

    public static SharedDto.PaymentScheduleSimpleItem ToApiResponseItem(this PaymentScheduleSimple resultItem)
        => new SharedDto.PaymentScheduleSimpleItem()
        {
            Amount = resultItem.Amount,
            Date = resultItem.Date,
            PaymentNumber = resultItem.PaymentNumber,
            Type = resultItem.Type,
        };

    public static SharedDto.MarketingActionItem ToApiResponseItem(this ResultMarketingAction resultItem)
        => new SharedDto.MarketingActionItem()
        {
            Code = resultItem.Code,
            Requested = resultItem.Requested == 1,
            Applied = resultItem.Applied == 1,
            MarketingActionId = resultItem.MarketingActionId,
            Deviation = resultItem.Deviation,
            Name = resultItem.Name
        };

    public static List<SharedDto.FeeItem> ToApiResponse(this RepeatedField<ResultFee> fees)
        => fees.Select(t => new SharedDto.FeeItem
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

    public static CreditWorthinessSimpleResults? ToApiResponse(this MortgageOfferCreditWorthinessSimpleResults? result)
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

    public static CreditWorthinessSimpleInputs? ToApiResponse(this MortgageOfferCreditWorthinessSimpleInputs? inputs, bool isActive)
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