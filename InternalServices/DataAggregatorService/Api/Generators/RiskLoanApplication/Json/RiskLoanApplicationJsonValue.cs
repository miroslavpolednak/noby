using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Helpers;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication.Json;

internal class RiskLoanApplicationJsonValue : RiskLoanApplicationJsonObject
{
    public required string DataFieldPath { get; init; }

    public required bool UseDefaultInsteadOfNull { get; init; }

    public override void Add(string[] propertyPath, string dataFieldPath, bool useDefaultInsteadOfNull)
    {
        throw new NotImplementedException();
    }

    public override object? GetJsonObject(object data)
    {
        var value = MapperHelper.GetValue(data, DataFieldPath);

        if (value is not null || !UseDefaultInsteadOfNull)
            return GetValue(value);
        
        var nullableType = Nullable.GetUnderlyingType(MapperHelper.GetType(data, DataFieldPath));

        return nullableType is not null ? Activator.CreateInstance(nullableType) : GetValue(value);
    }

    private static object? GetValue(object? value) =>
        value switch
        {
            GrpcDecimal grpcDecimal => (decimal)grpcDecimal,
            NullableGrpcDecimal nullableGrpcDecimal => (decimal?)nullableGrpcDecimal,
            GrpcDate grpcDate => (DateTime)grpcDate,
            NullableGrpcDate nullableGrpcDate => (DateTime?)nullableGrpcDate,
            _ => value
        };
}