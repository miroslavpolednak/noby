namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DocumentDataField
{
    public int DocumentId { get; set; }

    public int DocumentVersion { get; set; }

    public int DataFieldId { get; set; }

    public string TemplateFieldName { get; set; } = null!;

    public Document Document { get; set; } = null!;

    public DataField DataField { get; set; } = null!;
}