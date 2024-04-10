using CIS.Core.Attributes;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageExtraPaymentData;
using __Contracts = DomainServices.OfferService.Contracts;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class MortgageExtraPaymentDataMapper
{
    public __Contracts.MortgageExtraPaymentFullData MapToFullData(MortgageExtraPaymentData data)
    {
        return new __Contracts.MortgageExtraPaymentFullData
        {
            SimulationInputs = new()
            {
            },
            SimulationResults = new()
            {
            },
            BasicParameters = new()
            {
            }
        };
    }

    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageExtraPaymentBasicParameters basicParameters)
    {
        return new()
        {
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageRetentionSimulationInputs inputs)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRetentionBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRetentionSimulationInputs MapFromDataInputs(SimulationInputsData inputs)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRetentionSimulationResults MapFromDataOutputs(SimulationOutputsData output)
    {
        return new()
        {
        };
    }
}
