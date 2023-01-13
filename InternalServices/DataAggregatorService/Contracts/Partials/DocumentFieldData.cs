namespace CIS.InternalServices.DataAggregatorService.Contracts;

public partial class DocumentFieldData
{
    public object? GetValue() =>
        ValueCase switch
        {
            ValueOneofCase.None => default,
            ValueOneofCase.Text => Text,
            ValueOneofCase.Date => Date,
            ValueOneofCase.Number => Number,
            ValueOneofCase.DecimalNumber => DecimalNumber,
            ValueOneofCase.LogicalValue => LogicalValue,
            ValueOneofCase.Table => Table,
            _ => throw new ArgumentOutOfRangeException()
        };
}