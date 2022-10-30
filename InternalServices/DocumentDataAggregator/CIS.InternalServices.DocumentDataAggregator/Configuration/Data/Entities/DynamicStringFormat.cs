namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DynamicStringFormat
{
    public int DynamicStringFormatId { get; set; }

    public int DocumentId { get; set; }

    public int DocumentVersion { get; set; }

    public int DataFieldId { get; set; }

    public string Format { get; set; } = null!;

    public int Priority { get; set; }

    public ICollection<DynamicStringFormatCondition> DynamicStringFormatConditions { get; set; } = null!;
}