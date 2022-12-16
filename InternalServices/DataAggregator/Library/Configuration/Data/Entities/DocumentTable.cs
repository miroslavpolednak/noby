namespace CIS.InternalServices.DataAggregator.Configuration.Data.Entities;

internal class DocumentTable
{
    public int DocumentTableId { get; set; }

    public int DocumentId { get; set; }

    public string DocumentVersion { get; set; } = null!;

    public int DataFieldId { get; set; }

    public string AcroFieldPlaceholderName { get; set; } = null!;

    public string? ConcludingParagraph { get; set; }

    public Document Document { get; set; } = null!;

    public DataField DataField { get; set; } = null!;

    public ICollection<DocumentTableColumn> DocumentTableColumns { get; set; } = null!;
}