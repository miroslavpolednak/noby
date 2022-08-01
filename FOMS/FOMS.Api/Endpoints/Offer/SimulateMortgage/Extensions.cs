using DomainServices.OfferService.Contracts;
using Google.Protobuf.Collections;

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
                ExpectedDateOfDrawing = request.ExpectedDateOfDrawing,
                Developer = request.Developer is null ? null : new Developer
                {
                    DeveloperId = request.Developer.DeveloperId,
                    NewDeveloperCin = request.Developer.NewDeveloperCin,
                    NewDeveloperName = request.Developer.NewDeveloperName,
                    NewDeveloperProjectName = request.Developer.NewDeveloperProjectName,
                    ProjectId = request.Developer.ProjectId
                }
            },
            BasicParameters = new()
            {
                FinancialResourcesOwn = request.FinancialResourcesOwn,
                FinancialResourcesOther = request.FinancialResourcesOther,
                StatementTypeId = request.StatementTypeId
            }
        };

        if (request.LoanPurposes is not null && request.LoanPurposes.Any())
            model.SimulationInputs.LoanPurposes.AddRange(
                request.LoanPurposes.Select(t => new DomainServices.OfferService.Contracts.LoanPurpose() { LoanPurposeId = t.Id, Sum = t.Sum })
                );

        if (request.MarketingActions is not null)
        {
            model.SimulationInputs.MarketingActions = new InputMarketingAction()
            {
                Domicile = request.MarketingActions.Domicile,
                HealthRiskInsurance = request.MarketingActions.HealthRiskInsurance,
                RealEstateInsurance = request.MarketingActions.RealEstateInsurance,
                IncomeLoanRatioDiscount = request.MarketingActions.IncomeLoanRatioDiscount,
                UserVip = request.MarketingActions.UserVip,
            };
        }

        return model;
    }

    public static Dto.MortgageOutputs ToApiResponse(this MortgageSimulationResults result, MortgageSimulationInputs inputs)
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
            MarketingActions = result.MarketingActions?.Select(i => i.ToApiResponseItem()).ToList()
        };

    public static List<Dto.Fee> ToApiResponse(this RepeatedField<ResultFee> fees)
        => fees.Select(t => new Dto.Fee
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

    private static Dto.MarketingActionResult ToApiResponseItem(this ResultMarketingAction resultItem)
    {
        return new Dto.MarketingActionResult() {
            Code = resultItem.Code,
            Requested = resultItem.Requested == 1,
            Applied = resultItem.Applied == 1,
            MarketingActionId = resultItem.MarketingActionId, 
            Deviation = resultItem.Deviation
        };
    }
}