namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class NullableGrpcDecimal
{
    private const decimal NanoFactor = 1_000_000_000;
    public NullableGrpcDecimal(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator double?(NullableGrpcDecimal? grpcDecimal)
    {
        if (grpcDecimal == null) return default(double?);
        return Convert.ToDouble(grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor);
    }

    public static implicit operator decimal?(NullableGrpcDecimal? grpcDecimal)
    {
        if (grpcDecimal == null) return default(decimal?);
        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    }

    public static implicit operator NullableGrpcDecimal?(decimal? value)
    {
        if (!value.HasValue) return default(NullableGrpcDecimal);
        
        var units = decimal.ToInt64(value.Value);
        var nanos = decimal.ToInt32((value.Value - units) * NanoFactor);
        return new NullableGrpcDecimal(units, nanos);
    }
}
