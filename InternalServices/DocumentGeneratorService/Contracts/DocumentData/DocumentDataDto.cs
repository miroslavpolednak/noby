using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.InternalServices.DocumentGeneratorService.Contracts;

public class DocumentDataDto : IDocumentData
{
    int IDocumentData.ValueTypeId => ValueCase;

    public string FieldName { get; set; } = null!;

    public string? StringFormat { get; set; }

    public byte? TextAlign { get; set; }

    public byte? VAlign { get; set; }

    public string? Text { get; set; }

    public NullableGrpcDate? Date { get; set; }

    public int Number { get; set; }

    public NullableGrpcDecimal? DecimalNumber { get; set; }

    public bool LogicalValue { get; set; }

    public int ValueCase { get; set; }
}