using System.Collections;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents;

internal class DocumentMapper
{
    private readonly DocumentConfiguration _configuration;
    private readonly AggregatedData _aggregatedData;

    public DocumentMapper(DocumentConfiguration configuration, AggregatedData aggregatedData)
    {
        _configuration = configuration;
        _aggregatedData = aggregatedData;
    }

    public IEnumerable<DocumentFieldData> MapDocumentFieldData()
    {
        return Enumerable.Union(MapDocumentFields(), MapDocumentTables());
    }

    private IEnumerable<DocumentFieldData> MapDocumentFields()
    {
        var dynamicStringFormats = GetDynamicStringFormats();

        return _configuration.SourceFields
                             .GroupBy(f => CollectionPathHelper.GetCollectionPath(f.FieldPath))
                             .SelectMany(group => FieldParser.ISourceFieldParser.Create(group.Key).GetFields(group, _aggregatedData))
                             .Where(f => f.Value is not null && (f.Value is not string str || !string.IsNullOrWhiteSpace(str)))
                             .Select(ParseDocumentFieldData);

        DocumentFieldData ParseDocumentFieldData(DocumentSourceFieldData sourceData)
        {
            var stringFormat = sourceData.StringFormat;

            if (sourceData.SourceFieldId.HasValue && dynamicStringFormats.ContainsKey(sourceData.SourceFieldId.Value))
                stringFormat = dynamicStringFormats[sourceData.SourceFieldId.Value];

            var fieldData = new DocumentFieldData
            {
                FieldName = sourceData.AcroFieldName,
                StringFormat = stringFormat,
            };

            SetDocumentFieldDataValue(fieldData, sourceData.Value);

            return fieldData;
        }
    }

    private Dictionary<int, string> GetDynamicStringFormats() =>
        _configuration.DynamicStringFormats.Select(formats => new
                      {
                          formats.Key,
                          Format = DynamicStringFormatParser.ParseStringFormat(formats, _aggregatedData)
                      })
                      .Where(d => d.Format is not null)
                      .ToDictionary(d => d.Key, d => d.Format!);

    private void SetDocumentFieldDataValue(DocumentFieldData fieldData, object? value)
    {
        if (TrySetCommonValue(fieldData, value))
            return;

        switch (value)
        {
            case bool logicalValue:
                fieldData.LogicalValue = logicalValue;
                break;
        }
    }

    private IEnumerable<DocumentFieldData> MapDocumentTables()
    {
        foreach (var table in _configuration.Tables)
        {
            var collectionSource = MapperHelper.GetValue(_aggregatedData, table.CollectionSourcePath.Replace(ConfigurationConstants.CollectionMarker, ""));

            if (collectionSource is null)
                continue;

            if (collectionSource is not IEnumerable collection)
                throw new InvalidOperationException($"Path {table.CollectionSourcePath} does not return IEnumerable.");

            yield return new DocumentFieldData
            {
                FieldName = table.AcroFieldPlaceholder,
                Table = new GenericTable
                {
                    Columns =
                    {
                        table.Columns.Select(c => new GenericTableColumn
                        {
                            Header = c.Header,
                            WidthPercentage = (GrpcDecimal)c.WidthPercentage,
                            StringFormat = c.StringFormat
                        })
                    },
                    Rows = { GetTableRow(table.Columns, collection) },
                    ConcludingParagraph = table.ConcludingParagraph
                }
            };
        }
    }

    private static IEnumerable<GenericTableRow> GetTableRow(ICollection<DocumentTable.Column> columns, IEnumerable collection)
    {
        return collection.Cast<object>().Select(obj =>
        {
            var rowValues = (ICollection<object?>)columns.Select(c => MapperHelper.GetValue(obj, c.CollectionFieldPath)).ToList();

            return new GenericTableRow
            {
                Values =
                {
                    rowValues.Select(value =>
                    {
                        var tableRowValue = new GenericTableRowValue();

                        TrySetCommonValue(tableRowValue, value);

                        return tableRowValue;
                    })
                }
            };
        }).ToList();
    }
    
    private static bool TrySetCommonValue(ICommonDocumentFieldValue fieldValue, object? value)
    {
        switch (value)
        {
            case null:
                break;

            case string text:
                fieldValue.Text = text;
                break;

            case DateTime date:
                fieldValue.Date = date;
                break;

            case GrpcDate date:
                fieldValue.Date = date;
                break;

            case NullableGrpcDate nullableGrpcDate:
                fieldValue.Date = (GrpcDate)nullableGrpcDate;
                break;

            case int number:
                fieldValue.Number = number;
                break;

            case double doubleNumber:
                fieldValue.DecimalNumber = (GrpcDecimal)doubleNumber;
                break;

            case decimal decimalNumber:
                fieldValue.DecimalNumber = decimalNumber;
                break;

            case NullableGrpcDecimal nullableGrpcDecimal:
                fieldValue.DecimalNumber = (GrpcDecimal)nullableGrpcDecimal;
                break;

            case GrpcDecimal grpcDecimal:
                fieldValue.DecimalNumber = grpcDecimal;
                break;

            default:
                return false;
        }

        return true;
    }
}