using CIS.Core.Attributes;
using __Contracts = DomainServices.OfferService.Contracts;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageRefixationData;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class MortgageRefixationDataMapper
{
    public __Contracts.MortgageRefixationFullData MapToFullData(MortgageRefixationData data)
    {
        return new __Contracts.MortgageRefixationFullData
        {
            SimulationInputs = MapFromDataInputs(data.SimulationInputs),
            SimulationResults = MapFromDataOutputs(data.SimulationOutputs),
            BasicParameters = MapFromDataBasicParameters(data.BasicParameters)
        };
    }

    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageRefixationBasicParameters basicParameters)
    {
        return new()
        {
            FixedRateValidTo = basicParameters.FixedRateValidTo,
            LegalNoticeGeneratedDate = basicParameters.LegalNoticeGeneratedDate
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageRefixationSimulationInputs inputs)
    {
        return new()
        {
            FixedRatePeriod = inputs.FixedRatePeriod,
            InterestRate = inputs.InterestRate,
            InterestRateDiscount = inputs.InterestRateDiscount,
            InterestRateValidFrom = inputs.InterestRateValidFrom
        };
    }

    public __Contracts.MortgageRefixationBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
            FixedRateValidTo = basicParameters.FixedRateValidTo,
            LegalNoticeGeneratedDate = basicParameters.LegalNoticeGeneratedDate
        };
    }

    public __Contracts.MortgageRefixationSimulationInputs MapFromDataInputs(SimulationInputsData inputs)
    {
        return new()
        {
            FixedRatePeriod = inputs.FixedRatePeriod,
            InterestRate = inputs.InterestRate,
            InterestRateDiscount = inputs.InterestRateDiscount,
            InterestRateValidFrom = inputs.InterestRateValidFrom
        };
    }

    public __Contracts.MortgageRefixationSimulationResults MapFromDataOutputs(SimulationOutputsData output)
    {
        return new()
        {
            LoanPaymentAmount = output.LoanPaymentAmount,
            LoanPaymentAmountDiscounted = output.LoanPaymentAmountDiscounted,
            LoanPaymentsCount = output.LoanPaymentsCount,
            MaturityDate = output.MaturityDate
        };
    }
}
