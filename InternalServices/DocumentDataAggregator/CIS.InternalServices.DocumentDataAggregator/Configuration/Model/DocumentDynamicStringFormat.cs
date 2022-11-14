namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Model;

internal class DocumentDynamicStringFormat
{
    public int SourceFieldId { get; init; }

    public string Format { get; init; } = null!;

    public int Priority { get; init; }

    public ICollection<DocumentDynamicStringFormatCondition> Conditions { get; init; } = new List<DocumentDynamicStringFormatCondition>();
}