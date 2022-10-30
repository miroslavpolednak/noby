namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DynamicStringFormatCondition
{
    public int DynamicStringFormatId { get; set; }

    public int DynamicStringFormatDataFieldId { get; set; }

    public string EqualToValue { get; set; } = null!;

    public DynamicStringFormatDataField DynamicStringFormatDataField { get; set; } = null!;
}