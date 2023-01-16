namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class DocumentSpecialDataField
{
    public int DocumentId { get; set; }

    public string AcroFieldName { get; set; } = null!;

    public int DataServiceId { get; set; }

    public string FieldPath { get; set; } = null!;

    public string? StringFormat { get; set; }

    public Document Document { get; set; } = null!;

    public DataService DataService { get; set; } = null!;
}