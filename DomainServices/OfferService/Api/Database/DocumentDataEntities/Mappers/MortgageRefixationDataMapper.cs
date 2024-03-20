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

    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageRefixationBasicParameters basicParameters)
    {
        return new()
        {
        };
    }

    public SimulationInputsData MapToDataInputs(__Contracts.MortgageRefixationSimulationInputs inputs, decimal interestRate)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRefixationBasicParameters MapFromDataBasicParameters(BasicParametersData basicParameters)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRefixationSimulationInputs MapFromDataInputs(SimulationInputsData inputs)
    {
        return new()
        {
        };
    }

    public __Contracts.MortgageRefixationSimulationResults MapFromDataOutputs(SimulationOutputsData output)
    {
        return new()
        {
        };
    }
}
