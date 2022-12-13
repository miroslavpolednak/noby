namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DynamicStringFormat
{
    public int DynamicStringFormatId { get; set; }

    public int DocumentDataFieldId { get; set; }

    public string Format { get; set; } = null!;

    public int Priority { get; set; }

    public DocumentDataField DocumentDataField { get; set; } = null!;

    public ICollection<DynamicStringFormatCondition> DynamicStringFormatConditions { get; set; } = null!;
}