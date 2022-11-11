namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DocumentDataField
{
    public int DocumentDataFieldId { get; set; }

    public int DocumentId { get; set; }

    public string DocumentVersion { get; set; } = null!;

    public int DataFieldId { get; set; }

    public string TemplateFieldName { get; set; } = null!;

    public string? StringFormat { get; set; } = null!;

    public Document Document { get; set; } = null!;

    public DataField DataField { get; set; } = null!;

    public ICollection<DynamicStringFormat> DynamicStringFormats { get; set; } = null!;
}