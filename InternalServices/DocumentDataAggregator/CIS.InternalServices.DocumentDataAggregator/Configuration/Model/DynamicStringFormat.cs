namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DynamicStringFormat
{
    public int SourceFieldId { get; init; }

    public string Format { get; init; } = null!;

    public int Priority { get; init; }

    public ICollection<DynamicStringFormatCondition> Conditions { get; init; } = new List<DynamicStringFormatCondition>();
}