namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DynamicStringFormatCondition
{
    public string SourceFieldPath { get; init; } = null!;

    public string EqualToValue { get; init; } = null!;
}