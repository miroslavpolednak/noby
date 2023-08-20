namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentDynamicStringFormat
{
    public string AcroFieldName { get; init; } = null!;

    public string StringFormat { get; init; } = null!;

    public int Priority { get; init; }

    public ICollection<DocumentDynamicStringFormatCondition> Conditions { get; } = new List<DocumentDynamicStringFormatCondition>();
}