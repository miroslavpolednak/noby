using System.Collections;
using CIS.InternalServices.DataAggregator.Configuration.Document;
using CIS.InternalServices.DataAggregator.DataServices;
using CIS.InternalServices.DataAggregator.Documents.Table;
using CIS.InternalServices.DataAggregator.Helpers;
using DocumentTable = CIS.InternalServices.DataAggregator.Documents.Table.DocumentTable;

namespace CIS.InternalServices.DataAggregator.Documents.Mapper;

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

    public IEnumerable<DocumentFieldData> GetDocumentFields(IReadOnlyCollection<DocumentSourceField> sourceFields, IDictionary<int, string> dynamicStringFormats)
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
                FieldName = fieldData.AcroFieldName,
                Value = fieldData.Value!,
                StringFormat = stringFormat
            };
        }
    }

    public IEnumerable<DocumentFieldData> GetDocumentTables(IEnumerable<Configuration.Document.DocumentTable> documentTables)
    {
        foreach (var table in documentTables)
        {
            var collectionSource = MapperHelper.GetValue(_aggregatedData, table.CollectionSourcePath.Replace(ConfigurationConstants.CollectionMarker, ""));
            
            if (collectionSource is null)
                continue;

            if (collectionSource is not IEnumerable collection)
                throw new InvalidOperationException();

            var test = collection.Cast<object>().Select(obj =>
            {
                return (ICollection<object?>)table.Columns.Select(c => MapperHelper.GetValue(obj, c.CollectionFieldPath)).ToList();
            }).ToList();

            yield return new DocumentFieldData
            {
                FieldName = table.AcroFieldPlaceholder,
                Value = new DocumentTable
                {
                    Columns = table.Columns.Select(c => new DocumentTableColumn
                    {
                        Header = c.Header,
                        WidthPercentage = c.WidthPercentage,
                        StringFormat = c.StringFormat
                    }).ToList(),
                    RowsValues = test,
                    ConcludingParagraph = table.ConcludingParagraph
                }
            };
        }
    }

    private IEnumerable<SourceFieldData> GetSourceFieldDataSequence(IGrouping<string, DocumentSourceField> sourceFieldGroups)
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
        public required string AcroFieldName { get; init; }
        public string? StringFormat { get; init; }
        public required object? Value { get; init; }
    }
}