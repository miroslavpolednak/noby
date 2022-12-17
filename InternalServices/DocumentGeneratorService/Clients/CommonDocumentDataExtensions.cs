using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DocumentGeneratorService.Clients;

public static class CommonDocumentDataExtensions
{
    public static bool TrySetCommonData(this ICommonDocumentData documentData, object? obj)
    {
        switch (obj)
        {
            case null:
                break;

            case string text:
                documentData.Text = text;
                break;

            case DateTime date:
                documentData.Date = date;
                break;

            case GrpcDate date:
                documentData.Date = date;
                break;

            case NullableGrpcDate nullableGrpcDate:
                documentData.Date = (GrpcDate)nullableGrpcDate;
                break;

            case int number:
                documentData.Number = number;
                break;

            case double doubleNumber:
                documentData.DecimalNumber = (GrpcDecimal)doubleNumber;
                break;

            case decimal decimalNumber:
                documentData.DecimalNumber = decimalNumber;
                break;

            case NullableGrpcDecimal nullableGrpcDecimal:
                documentData.DecimalNumber = (GrpcDecimal)nullableGrpcDecimal;
                break;

            case GrpcDecimal grpcDecimal:
                documentData.DecimalNumber = grpcDecimal;
                break;

            default:
                return false;
        }

        return true;
    }
}