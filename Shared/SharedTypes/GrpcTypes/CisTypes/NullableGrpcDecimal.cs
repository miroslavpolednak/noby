﻿namespace SharedTypes.GrpcTypes;

public partial class NullableGrpcDecimal
{
    private const decimal NanoFactor = 1_000_000_000;

    public NullableGrpcDecimal(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator double?(NullableGrpcDecimal? grpcDecimal)
    {
        if (grpcDecimal == null) return default;

        return Convert.ToDouble(grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor);
    }

    public static implicit operator decimal?(NullableGrpcDecimal? grpcDecimal)
    {
        if (grpcDecimal == null) return default;

        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    }

    public static implicit operator NullableGrpcDecimal?(decimal value)
    {
        var units = decimal.ToInt64(value);
        var nanos = decimal.ToInt32((value - units) * NanoFactor);
        return new NullableGrpcDecimal(units, nanos);
    }

    public static implicit operator NullableGrpcDecimal?(decimal? value)
    {
        if (!value.HasValue) return default;
        
        var units = decimal.ToInt64(value.Value);
        var nanos = decimal.ToInt32((value.Value - units) * NanoFactor);
        return new NullableGrpcDecimal(units, nanos);
    }
}