using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace NOBY.Api.Endpoints.Document.Shared;

public class DocumentFieldDataWrapper : IDocumentData
{
    private readonly DocumentFieldData _fieldData;

    public DocumentFieldDataWrapper(DocumentFieldData fieldData)
    {
        _fieldData = fieldData;
    }

    public int ValueTypeId => (int)_fieldData.ValueCase;

    public string FieldName => _fieldData.FieldName;

    public string? StringFormat => _fieldData.StringFormat;

    public byte? TextAlign => (byte?)_fieldData.TextAlign;

    public byte? VAlign => (byte?)_fieldData.VAlign;

    public string? Text => _fieldData.Text;

    public NullableGrpcDate Date => new(_fieldData.Date.Year, _fieldData.Date.Month, _fieldData.Date.Day);

    public int Number => _fieldData.Number;
    public NullableGrpcDecimal DecimalNumber => _fieldData.DecimalNumber;

    public bool LogicalValue => _fieldData.LogicalValue;
}