namespace CIS.InternalServices.DataAggregatorService.Contracts;

public interface ICommonDocumentFieldValue
{
    string Text { get; set; }

    SharedTypes.GrpcTypes.GrpcDate Date { get; set; }

    int Number { get; set; }

    SharedTypes.GrpcTypes.GrpcDecimal DecimalNumber { get; set; }
}