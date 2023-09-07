namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.Result;

public class StaticValues
{
    private StaticValues()
    {
    }

    public static StaticValues Instance { get; } = new();

    public int DefaultOneValue => 1;

    public int DefaultZeroValue => 0;

    public bool DefaultFalseValue => false;
}