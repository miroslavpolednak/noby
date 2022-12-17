namespace CIS.InternalServices.DataAggregator.Configuration.Data.Entities;

internal class EasFormSpecialDataField
{
    public int EasRequestTypeId { get; set; }

    public int EasFormTypeId { get; set; }

    public string JsonPropertyName { get; set; } = null!;

    public int DataServiceId { get; set; }

    public string FieldPath { get; set; } = null!;

    public EasRequestType EasRequestType { get; set; } = null!;

    public EasFormType EasFormType { get; set; } = null!;

    public DataService DataService { get; set; } = null!;
}