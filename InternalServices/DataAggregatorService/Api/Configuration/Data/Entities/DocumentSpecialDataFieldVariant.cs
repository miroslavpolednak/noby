namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class DocumentSpecialDataFieldVariant
{
    public int DocumentId { get; set; }

    public string AcroFieldName { get; set; } = null!;

    public string DocumentVariant { get; set; } = null!;

    public DocumentSpecialDataField DocumentSpecialDataField { get; set; } = null!;
}