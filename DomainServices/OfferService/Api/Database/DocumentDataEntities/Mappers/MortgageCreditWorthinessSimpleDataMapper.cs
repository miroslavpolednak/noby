using CIS.Core.Attributes;
using DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using __Contracts = DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static

[TransientService, SelfService]
internal sealed class MortgageCreditWorthinessSimpleDataMapper
{
    public MortgageCreditWorthinessSimpleData MapToData(__Contracts.MortgageOfferCreditWorthinessSimpleInputs inputs, CreditWorthinessSimpleCalculateResponse? serviceResponse)
    {
        return new MortgageCreditWorthinessSimpleData
        {
            Inputs = new()
            {
                ChildrenCount = inputs.ChildrenCount,
                TotalMonthlyIncome = inputs.TotalMonthlyIncome,
                ExpensesSummary = new()
                {
                    Other = inputs.ExpensesSummary?.Other,
                    Rent = inputs.ExpensesSummary?.Rent
                },
                ObligationsSummary = new()
                {
                    AuthorizedOverdraftsTotalAmount = inputs.ObligationsSummary?.AuthorizedOverdraftsTotalAmount,
                    CreditCardsAmount = inputs.ObligationsSummary?.CreditCardsAmount,
                    LoansInstallmentsAmount = inputs.ObligationsSummary?.LoansInstallmentsAmount
                }
            },
            Outputs = new()
            {
                InstallmentLimit = (int?)serviceResponse?.InstallmentLimit,
                MaxAmount = (int?)serviceResponse?.MaxAmount,
                RemainsLivingAnnuity = (int?)serviceResponse?.RemainsLivingAnnuity,
                RemainsLivingInst = (int?)serviceResponse?.RemainsLivingInst,
                WorthinessResult = (MortgageCreditWorthinessSimpleData.WorthinessResults)(int)(serviceResponse?.WorthinessResult ?? CreditWorthinessResults.Unknown)
            }
        };
    }

    public (__Contracts.MortgageOfferCreditWorthinessSimpleInputs? Inputs, __Contracts.MortgageOfferCreditWorthinessSimpleResults? Outputs) MapFromDataToSingle(MortgageCreditWorthinessSimpleData? data)
    {
        if (data is null)
        {
            return (null, null);
        }

        var inputs = new __Contracts.MortgageOfferCreditWorthinessSimpleInputs
        {
            ChildrenCount = data.Inputs.ChildrenCount,
            TotalMonthlyIncome = data.Inputs.TotalMonthlyIncome,
            ExpensesSummary = new()
            {
                Other = data.Inputs.ExpensesSummary?.Other,
                Rent = data.Inputs.ExpensesSummary?.Rent
            },
            ObligationsSummary = new()
            {
                AuthorizedOverdraftsTotalAmount = data.Inputs.ObligationsSummary?.AuthorizedOverdraftsTotalAmount,
                CreditCardsAmount = data.Inputs.ObligationsSummary?.CreditCardsAmount,
                LoansInstallmentsAmount = data.Inputs.ObligationsSummary?.LoansInstallmentsAmount
            }
        };

        var results = new __Contracts.MortgageOfferCreditWorthinessSimpleResults
        {
            InstallmentLimit = data.Outputs.InstallmentLimit,
            MaxAmount = data.Outputs.MaxAmount,
            RemainsLivingAnnuity = data.Outputs.RemainsLivingAnnuity,
            RemainsLivingInst = data.Outputs.RemainsLivingInst,
            WorthinessResult = (__Contracts.MortgageOfferCreditWorthinessSimpleResults.Types.WorthinessResults)(int)data.Outputs.WorthinessResult
        };

        return (inputs, results);
    }
}
