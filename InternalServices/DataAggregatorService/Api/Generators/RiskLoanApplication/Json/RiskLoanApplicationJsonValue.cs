using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal class RiskLoanApplicationJsonValue : RiskLoanApplicationJsonObject
{
    public required string DataFieldPath { get; init; }

    public override void Add(string[] propertyPath, string dataFieldPath)
    {
        throw new NotImplementedException();
    }

    public override object? GetJsonObject(object data)
    {
        var value = MapperHelper.GetValue(data, DataFieldPath);

        return value switch
        {
            GrpcDecimal grpcDecimal => (decimal)grpcDecimal,
            NullableGrpcDecimal nullableGrpcDecimal => (decimal?)nullableGrpcDecimal,
            GrpcDate grpcDate => (DateTime)grpcDate,
            NullableGrpcDate nullableGrpcDate => (DateTime?)nullableGrpcDate,
            _ => value
        };
    }
}