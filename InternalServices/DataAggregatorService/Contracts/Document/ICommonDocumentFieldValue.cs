namespace CIS.InternalServices.DataAggregatorService.Contracts;

public interface ICommonDocumentFieldValue
{
    string Text { get; set; }

    Infrastructure.gRPC.CisTypes.GrpcDate Date { get; set; }

    int Number { get; set; }

    Infrastructure.gRPC.CisTypes.GrpcDecimal DecimalNumber { get; set; }
}