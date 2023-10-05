using SharedTypes.GrpcTypes;

namespace CIS.InternalServices.DocumentGeneratorService.Contracts;

public interface IDocumentData
{
    int ValueTypeId { get; }

    string FieldName { get; }

    string? StringFormat { get; }

    byte? TextAlign { get; }

    byte? VAlign { get; }

    string? Text { get; }

    NullableGrpcDate? Date { get; }

    int Number { get; }

    NullableGrpcDecimal? DecimalNumber { get; }

    bool LogicalValue { get; }
}