using CIS.Core.Attributes;
using __Contracts = DomainServices.OfferService.Contracts;
using static DomainServices.OfferService.Api.Database.DocumentDataEntities.MortgageRetentionData;
using ExternalServices.EasSimulationHT.V1.EasSimulationHTWrapper;

#pragma warning disable CA1822 // Mark members as static

namespace DomainServices.OfferService.Api.Database.DocumentDataEntities.Mappers;

[TransientService, SelfService]
internal sealed class MortgageRetentionDataMapper
{
    public BasicParametersData MapToDataBasicParameters(__Contracts.MortgageRetentionBasicParameters basicParameters)
    {
        return new()
        {
            Amount = basicParameters.Amount,
            AmountIndividualPrice = basicParameters.AmountIndividualPrice
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

    public SimulationOutputsData MapToDataOutputs(SimulationHTResponse results)
    {

    }
}
