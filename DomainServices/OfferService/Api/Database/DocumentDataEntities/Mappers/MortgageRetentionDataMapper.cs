﻿using CIS.Core.Attributes;
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
            SimulationInputs = MapFromDataInputs(data.SimulationInputs),
            SimulationResults = MapFromDataOutputs(data.SimulationOutputs),
            BasicParameters = MapFromDataBasicParameters(data.BasicParameters)
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

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageRetentionSimulationInputs inputs)
    {
        return new()
        {
            InterestRate = inputs.InterestRate,
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
