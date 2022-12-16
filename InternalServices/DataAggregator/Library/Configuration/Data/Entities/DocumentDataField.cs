namespace CIS.InternalServices.DataAggregator.Configuration.Data.Entities;

internal class DocumentDataField
{
    public int DocumentDataFieldId { get; set; }

    public int DocumentId { get; set; }

    public string DocumentVersion { get; set; } = null!;

    public int DataFieldId { get; set; }

    public string AcroFieldName { get; set; } = null!;

    public string? StringFormat { get; set; }

    public string? DefaultTextIfNull { get; set; }

    public Document Document { get; set; } = null!;

    public DataField DataField { get; set; } = null!;

    public ICollection<DynamicStringFormat> DynamicStringFormats { get; set; } = null!;
}