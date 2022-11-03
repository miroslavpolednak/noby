using DomainServices.OfferService.Contracts;
using Google.Protobuf.Collections;

namespace NOBY.Api.Endpoints.Offer;

internal static class OfferApiModuleDtoExtensions
{
    public static Dto.MortgageInputs ToApiResponse(this MortgageSimulationInputs input, BasicParameters basicParams)
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
            StatementTypeId = basicParams.StatementTypeId,
            LoanPurposes = input.LoanPurposes?.Select(t => new Dto.LoanPurposeItem() { Id = t.LoanPurposeId, Sum = t.Sum }).ToList(),
            Developer = input.Developer is null ? null : new Dto.Developer
            {
                DeveloperId = input.Developer.DeveloperId,
                NewDeveloperProjectName = input.Developer.NewDeveloperName,
                NewDeveloperCin = input.Developer.NewDeveloperCin,
                NewDeveloperName = input.Developer.NewDeveloperName,
                ProjectId = input.Developer.ProjectId
            },
            DrawingDurationId = input.DrawingDurationId,
            DrawingTypeId = input.DrawingTypeId,
            InterestRateDiscount = input.InterestRateDiscount,
            MarketingActions = input.MarketingActions is null ? null : new Dto.MarketingActionInputItem
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
            Fees = additionalResults.Fees is null ? null : additionalResults.Fees.ToApiResponse()
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
}