namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class DocumentDataFieldVariant
{
    public int DocumentDataFieldId { get; set; }

    public string DocumentVariant { get; set; } = null!;

    public DocumentDataField DocumentDataField { get; set; } = null!;
}