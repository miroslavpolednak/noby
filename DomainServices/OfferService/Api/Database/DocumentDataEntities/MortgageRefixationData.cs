namespace DomainServices.OfferService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class MortgageRefixationData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public SimulationInputsData SimulationInputs { get; set; }
    public SimulationOutputsData SimulationOutputs { get; set; }
    public BasicParametersData BasicParameters { get; set; }

    public sealed class SimulationInputsData
    {
        
    }

    public sealed class SimulationOutputsData
    {
        
    }

    public sealed class BasicParametersData
    {
        
    }
}
