namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class DynamicStringFormatCondition
{
    public int DynamicStringFormatId { get; set; }

    public int DataFieldId { get; set; }

    public string? EqualToValue { get; set; }

    public DataField DataField { get; set; } = null!;
}