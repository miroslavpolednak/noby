﻿using SharedTypes.GrpcTypes;
using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

internal static class Extensions
{
    public static DomainServices.OfferService.Contracts.SimulateMortgageRequest ToDomainServiceRequest(this OfferSimulateMortgageRequest request, DateTime guaranteeDateFrom, bool isUserVip)
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
                    Description = request.Developer.Description,
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
                FinancialResourcesOther = request.FinancialResourcesOther
            },
            IsCreditWorthinessSimpleRequested = request.CreditWorthinessSimpleInputs?.IsActive ?? false,
            CreditWorthinessSimpleInputs = request.CreditWorthinessSimpleInputs is null ? null : new MortgageOfferCreditWorthinessSimpleInputs
            {
                TotalMonthlyIncome = request.CreditWorthinessSimpleInputs.TotalMonthlyIncome,
                ExpensesSummary = new MortgageOfferCreditWorthinessSimpleInputs.Types.ExpensesSummaryObject
                {
                    Rent = request.CreditWorthinessSimpleInputs.ExpensesRent,
                    Other = request.CreditWorthinessSimpleInputs.ExpensesOther
                },
                ObligationsSummary = new MortgageOfferCreditWorthinessSimpleInputs.Types.ObligationsSummaryObject
                {
                    LoansInstallmentsAmount = request.CreditWorthinessSimpleInputs.LoansInstallmentsAmount,
                    CreditCardsAmount = request.CreditWorthinessSimpleInputs.CreditCardsAmount,
                    AuthorizedOverdraftsTotalAmount = request.CreditWorthinessSimpleInputs.AuthorizedOverdraftsTotalAmount
                },
                ChildrenCount = request.CreditWorthinessSimpleInputs.ChildrenCount
            },
            Identities = { request.CustomerIdentities?.Select(c=> new Identity(c)) ?? Enumerable.Empty<Identity>() }
        };

        if (request.Fees is not null)
        {
            model.SimulationInputs.Fees.AddRange(request.Fees.Select(f => new InputFee
            {
                DiscountPercentage = f.DiscountPercentage,
                FeeId = f.FeeId
            }));
        }

        if (request.LoanPurposes is not null && request.LoanPurposes.Count != 0)
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
                UserVip = isUserVip,
            };
        }

        return model;
    }
}