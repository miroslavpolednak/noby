namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class EasFormDynamicInputParameter
{
    public int EasRequestTypeId { get; set; }

    public int EasFormTypeId { get; set; }

    public int InputParameterId { get; set; }

    public int TargetDataServiceId { get; set; }

    public int SourceDataFieldId { get; set; }

    public EasRequestType EasRequestType { get; set; } = null!;

    public EasFormType EasFormType { get; set; } = null!;

    public InputParameter InputParameter { get; set; } = null!;

    public DataService TargetDataService { get; set; } = null!;

    public DataField SourceDataField { get; set; } = null!;
}