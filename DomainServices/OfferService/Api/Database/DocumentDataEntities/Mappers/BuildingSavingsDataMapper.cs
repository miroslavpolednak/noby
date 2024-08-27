using CIS.Core.Attributes;
using DomainServices.OfferService.Contracts;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.BuildingSavingsData;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class BuildingSavingsDataMapper
{
    public SimulationInputsData MapToDataInputs(BuildingSavingsSimulationInputs inputs)
    {
        return new SimulationInputsData
        {
            MarketingActionCode = inputs.MarketingActionCode,
            TargetAmount = inputs.TargetAmount,
            MinimumMonthlyDeposit = inputs.MinimumMonthlyDeposit,
            ContractStartDate = inputs.ContractStartDate,
            SimulateUntilBindingPeriod = inputs.SimulateUntilBindingPeriod,
            ContractTerminationDate = inputs.ContractTerminationDate,
            AnnualStatementRequired = inputs.AnnualStatementRequired,
            StateSubsidyRequired = inputs.StateSubsidyRequired,
            IsClientSVJ = inputs.IsClientSVJ,
            IsClientJuridicalPerson = inputs.IsClientJuridicalPerson,
            ClientDateOfBirth = inputs.ClientDateOfBirth,
            ExtraDeposits = inputs.ExtraDeposits.Select(e => new SimulationInputsData.ExtraDeposit
            {
                Date = e.Date,
                Amount = e.Amount
            }).ToList()
        };
    }

    public SimulationOutputsData MapToDataOutputs(BuildingSavingsSimulationResults results)
    {
        return new SimulationOutputsData
        {
            SavingsLengthInMonths = results.SavingsLengthInMonths,
            InterestRate = results.InterestRate,
            SavingsSum = results.SavingsSum,
            DepositsSum = results.DepositsSum,
            InterestsSum = results.InterestsSum,
            FeesSum = results.FeesSum,
            BonusInterestRate = results.BonusInterestRate,
            StateSubsidySum = results.StateSubsidySum,
            InterestBenefitAmount = results.InterestBenefitAmount,
            InterestBenefitTax = results.InterestBenefitTax
        };
    }
}