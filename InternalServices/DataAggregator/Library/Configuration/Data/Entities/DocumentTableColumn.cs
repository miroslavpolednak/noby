namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DocumentTableColumn
{
    public int DocumentTableId { get; set; }

    public string FieldPath { get; set; } = null!;

    public int Order { get; set; }

    public string Header { get; set; } = null!;

    public float WidthPercentage { get; set; }

    public string? StringFormat { get; set; }

    public DocumentTable DocumentTable { get; set; } = null!;
}