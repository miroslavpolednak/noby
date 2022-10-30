using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public static class DocumentPartDataExtensions
{
    public static GenerateDocumentPartData SetDocumentPartDataValue(this GenerateDocumentPartData partData, object value)
    {
        switch (value)
        {
            case string text:
                partData.Text = text;
                break;

            case GrpcDate date:
                partData.Date = date;
                break;

            case NullableGrpcDate nullableGrpcDate:
                partData.Date = (GrpcDate)nullableGrpcDate;
                break;

            case int number:
                partData.Number = number;
                break;

            case NullableGrpcDecimal nullableGrpcDecimal:
                partData.DecimalNumber = (GrpcDecimal)nullableGrpcDecimal;
                break;

            case GrpcDecimal grpcDecimal:
                partData.DecimalNumber = grpcDecimal;
                break;
        }

        return partData;
    }
}