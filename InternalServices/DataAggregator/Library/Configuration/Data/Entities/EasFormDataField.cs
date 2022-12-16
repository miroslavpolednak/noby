namespace CIS.InternalServices.DataAggregator.Configuration.Data.Entities;

internal class EasFormDataField
{
    public int EasFormDataFieldId { get; set; }

    public int EasRequestTypeId { get; set; }

    public int DataFieldId { get; set; }

    public int EasFormTypeId { get; set; }

    public string JsonPropertyName { get; set; } = null!;

    public EasRequestType EasRequestType { get; set; } = null!;

    public DataField DataField { get; set; } = null!;

    public EasFormType EasFormType { get; set; } = null!;
}