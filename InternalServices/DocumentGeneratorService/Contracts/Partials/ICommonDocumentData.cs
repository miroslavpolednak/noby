namespace CIS.InternalServices.DocumentGeneratorService.Contracts;

public interface ICommonDocumentData
{
    string Text { get; set; }
    global::CIS.Infrastructure.gRPC.CisTypes.GrpcDate Date { get; set; }
    int Number { get; set; }
    global::CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal DecimalNumber { get; set; }
}