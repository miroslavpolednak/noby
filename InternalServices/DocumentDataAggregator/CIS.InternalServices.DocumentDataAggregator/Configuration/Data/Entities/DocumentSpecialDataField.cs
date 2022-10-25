namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data.Entities;

internal class DocumentSpecialDataField
{
    public int DocumentId { get; set; }

    public string FieldPath { get; set; } = null!;

    public int DataServiceId { get; set; }

    public string TemplateFieldName { get; set; } = null!;

    public Document Document { get; set; } = null!;

    public DataService DataService { get; set; } = null!;
}