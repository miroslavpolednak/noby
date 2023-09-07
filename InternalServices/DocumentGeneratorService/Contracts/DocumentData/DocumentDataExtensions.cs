using CIS.Infrastructure.gRPC.CisTypes;
using ValueType = CIS.InternalServices.DocumentGeneratorService.Contracts.GenerateDocumentPartData.ValueOneofCase;

namespace CIS.InternalServices.DocumentGeneratorService.Contracts;

public static class DocumentDataExtensions
{
    public static IEnumerable<GenerateDocumentPartData> CreateDocumentData(this IEnumerable<IDocumentData> documentData) =>
        documentData.Select(data => CreateDocumentData(data).SetValue(data));

    public static GenerateDocumentPartData CreateDocumentData(this IDocumentData documentData) =>
        new()
        {
            Key = documentData.FieldName,
            StringFormat = documentData.StringFormat,
            TextAlign = (TextAlign?)documentData.TextAlign ?? TextAlign.Unkwnon,
            VAlign = (VAlign?)documentData.VAlign ?? VAlign.Unknown
        };

    public static GenerateDocumentPartData SetValue(this GenerateDocumentPartData documentData, IDocumentData sourceData)
    {
        var result = documentData.TrySetValue(sourceData);

        if (result)
            return documentData;

        if (sourceData.ValueTypeId == (int)ValueType.Table)
            throw new NotSupportedException($"The {nameof(SetValue)} does not support the GenericTable type. Use the {nameof(TrySetValue)} instead and map the table manually.");

        throw new NotSupportedException($"Not supported OneOf ({sourceData.ValueTypeId}) object");
    }

    public static bool TrySetValue(this GenerateDocumentPartData documentData, IDocumentData sourceData)
    {
        switch ((ValueType)sourceData.ValueTypeId)
        {
            case ValueType.None: break; //Should be just a StringFormat, the DataAggregator sends only the necessary data 

            case ValueType.Text:
                documentData.Text = sourceData.Text ?? string.Empty;
                break;
            case ValueType.Date:
                documentData.Date = (GrpcDate)sourceData.Date!;
                break;
            case ValueType.Number:
                documentData.Number = sourceData.Number;
                break;
            case ValueType.DecimalNumber:
                documentData.DecimalNumber = (GrpcDecimal)sourceData.DecimalNumber!;
                break;
            case ValueType.LogicalValue:
                documentData.LogicalValue = sourceData.LogicalValue;
                break;

            case ValueType.Table:
            default: return false;
        }

        return true;
    }
}