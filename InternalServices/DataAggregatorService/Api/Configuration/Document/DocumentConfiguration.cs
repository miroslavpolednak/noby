namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

internal class DocumentConfiguration : ConfigurationBase<DocumentSourceField>
{
    public required int DocumentTemplateVersionId { get; init; }

    public required int? DocumentTemplateVariantId { get; init; }

    public required ILookup<string, DocumentDynamicStringFormat> DynamicStringFormats { get; init; }

    public required IReadOnlyCollection<DocumentTable> Tables { get; init; }

    protected override IEnumerable<DataService> GetDataServices()
    {
        var tableDataServices = Tables.Select(t => t.DataService);

        return base.GetDataServices().Concat(tableDataServices);
    }
}