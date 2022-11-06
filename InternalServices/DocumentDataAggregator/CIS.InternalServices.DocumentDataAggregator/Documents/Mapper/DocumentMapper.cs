using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Mapper;

namespace CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;

internal class DocumentMapper
{
    private readonly AggregatedData _aggregatedData;

    private DocumentMapper(AggregatedData aggregatedData)
    {
        _aggregatedData = aggregatedData;
    }

    public static DocumentMapper Create(AggregatedData aggregatedData) => new(aggregatedData);

    public Dictionary<int, string> GetDynamicStringFormats(ILookup<int, DocumentDynamicStringFormat> dynamicStringFormats) =>
        dynamicStringFormats.Select(formats => new
                            {
                                formats.Key,
                                Format = DynamicStringFormatParser.ParseStringFormat(formats, _aggregatedData)
                            })
                            .Where(d => d.Format is not null)
                            .ToDictionary(d => d.Key, d => d.Format!);

    public IEnumerable<DocumentFieldData> GetDocumentFields(IReadOnlyCollection<SourceField> sourceFields, IDictionary<int, string> dynamicStringFormats)
    {
        return sourceFields.GroupBy(f => CollectionPathHelper.GetCollectionPath(f.FieldPath))
                           .SelectMany(GetSourceFieldDataSequence)
                           .Select(ParseDocumentFieldData);

        DocumentFieldData ParseDocumentFieldData(SourceFieldData fieldData)
        {
            var stringFormat = fieldData.StringFormat;

            if (fieldData.SourceFieldId.HasValue && dynamicStringFormats.ContainsKey(fieldData.SourceFieldId.Value))
                stringFormat = dynamicStringFormats[fieldData.SourceFieldId.Value];

            return new DocumentFieldData
            {
                FieldName = fieldData.TemplateFieldName,
                Value = fieldData.Value!,
                StringFormat = stringFormat
            };
        }
    }

    private IEnumerable<SourceFieldData> GetSourceFieldDataSequence(IGrouping<string, SourceField> sourceFieldGroups)
    {
        ISourceFieldParser valueParser = sourceFieldGroups.Key == string.Empty ? new SingleValueFieldParser() : new CollectionFieldParser();

        return valueParser.GetFields(sourceFieldGroups, _aggregatedData).Where(FilterEmptyFields);

        static bool FilterEmptyFields(SourceFieldData fieldData) =>
            fieldData.Value switch
            {
                null => false,
                string text => !string.IsNullOrWhiteSpace(text),
                _ => true
            };
    }

    internal class SourceFieldData
    {
        public int? SourceFieldId { get; init; }
        public string TemplateFieldName { get; init; } = null!;
        public string? StringFormat { get; init; }
        public object? Value { get; init; }
    }
}