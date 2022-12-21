using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

internal static class Extensions
{
    public static DomainServices.OfferService.Contracts.SimulateMortgageRequest ToDomainServiceRequest(this SimulateMortgageRequest request, DateTime guaranteeDateFrom)
    {
        var model = new DomainServices.OfferService.Contracts.SimulateMortgageRequest()
        {
            ResourceProcessId = request.ResourceProcessId,
            SimulationInputs = new()
            {
                GuaranteeDateFrom = guaranteeDateFrom,
                InterestRateDiscount = request.InterestRateDiscount,
                DrawingTypeId = request.DrawingTypeId,
                DrawingDurationId = request.DrawingDurationId,
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
                },
                RealEstateInsurance = request.RealEstateInsurance is null ? null : new RealEstateInsurance
                {
                    Sum = request.RealEstateInsurance.Sum,
                    Frequency = request.RealEstateInsurance.Frequency
                },
                RiskLifeInsurance = request.RiskLifeInsurance is null ? null : new RiskLifeInsurance
                {
                    Sum = request.RiskLifeInsurance.Sum,
                    Frequency = request.RiskLifeInsurance.Frequency
                }
            },
            BasicParameters = new()
            {
                FinancialResourcesOwn = request.FinancialResourcesOwn,
                FinancialResourcesOther = request.FinancialResourcesOther,
                StatementTypeId = request.StatementTypeId
            }
        };

        if (request.Fees is not null)
        {
            model.SimulationInputs.Fees.AddRange(request.Fees.Select(f => new InputFee
            {
                DiscountPercentage = f.DiscountPercentage,
                FeeId = f.FeeId
            }));
        }

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
}