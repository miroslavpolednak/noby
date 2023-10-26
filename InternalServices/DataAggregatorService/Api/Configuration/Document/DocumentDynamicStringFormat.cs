namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentDynamicStringFormat
{
    public int DynamicStringFormatId { get; init; }

    public string AcroFieldName { get; init; } = null!;

    public string StringFormat { get; init; } = null!;

    public int Priority { get; init; }

    public List<DocumentDynamicStringFormatCondition> Conditions { get; } = new List<DocumentDynamicStringFormatCondition>();
}