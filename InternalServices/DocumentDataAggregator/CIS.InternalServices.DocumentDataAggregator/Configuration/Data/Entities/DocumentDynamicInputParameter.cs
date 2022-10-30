namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DocumentDynamicInputParameter
{
    public int DocumentId { get; set; }

    public string DocumentVersion { get; set; } = null!;

    public int InputParameterId { get; set; }

    public int TargetDataServiceId { get; set; }

    public int SourceDataFieldId { get; set; }

    public Document Document { get; set; } = null!;

    public InputParameter InputParameter { get; set; } = null!;

    public DataService TargetDataService { get; set; } = null!;

    public DataField SourceDataField { get; set; } = null!;
}