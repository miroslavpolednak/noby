using CIS.Core.Attributes;
using __Contracts = DomainServices.OfferService.Contracts;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageRetentionData;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class MortgageRetentionDataMapper
{
    public __Contracts.MortgageRetentionFullData MapToFullData(MortgageRetentionData data)
    {
        return new __Contracts.MortgageRetentionFullData
        {
            SimulationInputs = new()
            {
                InterestRateDiscount = data.SimulationInputs.InterestRateDiscount,
                InterestRate = data.SimulationInputs.InterestRate,
                InterestRateValidFrom = data.SimulationInputs.InterestRateValidFrom
            },
            SimulationResults = new()
            {
                LoanPaymentAmount = data.SimulationOutputs.LoanPaymentAmount,
                LoanPaymentAmountDiscounted = data.SimulationOutputs.LoanPaymentAmountDiscounted
            },
            BasicParameters = new()
            {
                FeeAmount = data.BasicParameters.FeeAmount,
                FeeAmountDiscounted = data.BasicParameters.FeeAmountDiscounted
            }
        };
    }

    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageRetentionBasicParameters basicParameters)
    {
        return new()
        {
            FeeAmount = basicParameters.FeeAmount,
            FeeAmountDiscounted = basicParameters.FeeAmountDiscounted
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageRetentionSimulationInputs inputs, decimal interestRate)
    {
        return new()
        {
            InterestRate = interestRate,
            InterestRateDiscount = inputs.InterestRateDiscount,
            InterestRateValidFrom = inputs.InterestRateValidFrom
        };
    }

    public __Contracts.MortgageRetentionBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
            FeeAmount = basicParameters.FeeAmount,
            FeeAmountDiscounted = basicParameters.FeeAmountDiscounted
        };
    }

    public __Contracts.MortgageRetentionSimulationInputs MapFromDataInputs(SimulationInputsData inputs)
    {
        return new()
        {
            InterestRate = inputs.InterestRate,
            InterestRateDiscount = inputs.InterestRateDiscount,
            InterestRateValidFrom = inputs.InterestRateValidFrom
        };
    }

    public __Contracts.MortgageRetentionSimulationResults MapFromDataOutputs(SimulationOutputsData output)
    {
        return new()
        {
            LoanPaymentAmount = output.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = output.LoanPaymentAmountDiscounted
        };
    }
}
