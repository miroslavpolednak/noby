using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

internal class RiskLoanApplicationJsonValueSource : JsonValueSource<RiskLoanApplicationSourceField>
{
    public RiskLoanApplicationJsonValueSource(RiskLoanApplicationSourceField sourceField) : base(sourceField)
    {
    }

    public static implicit operator RiskLoanApplicationJsonValueSource(RiskLoanApplicationSourceField sourceField) => new(sourceField);

    public override object? ParseValue(object? value, object sourceData)
    {
        if (value is not null || !SourceField.UseDefaultInsteadOfNull)
            return GetValue(value);

        var nullableType = Nullable.GetUnderlyingType(MapperHelper.GetType(sourceData, FieldPath));

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