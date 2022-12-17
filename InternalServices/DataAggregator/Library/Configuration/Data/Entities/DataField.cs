namespace CIS.InternalServices.DataAggregator.Configuration.Data.Entities;

internal class DataField
{
    public int DataFieldId { get; set; }

    public int DataServiceId { get; set; }

    public string FieldPath { get; set; } = null!;

    public string? DefaultStringFormat { get; set; }

    public DataService DataService { get; set; } = null!;
}