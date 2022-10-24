namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DataField
{
    public int DataFieldId { get; set; }

    public int DataServiceId { get; set; }

    public string FieldPath { get; set; } = null!;

    public DataService DataService { get; set; } = null!;
}