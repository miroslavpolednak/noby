﻿namespace SharedTypes.GrpcTypes;

public partial class GrpcDecimal
{
    private const decimal NanoFactor = 1_000_000_000;

    public GrpcDecimal(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator decimal?(GrpcDecimal? grpcDecimal)
    {
        if (grpcDecimal == null) return null;
        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    }

    public static implicit operator decimal(GrpcDecimal grpcDecimal)
    {
        if (grpcDecimal == null) return 0;
        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    }

    public static explicit operator double(GrpcDecimal grpcDecimal)
    {
        if (grpcDecimal == null) return 0;
        return Convert.ToDouble(grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor);
    }

    public static implicit operator GrpcDecimal(decimal value)
    {
        var units = decimal.ToInt64(value);
        var nanos = decimal.ToInt32((value - units) * NanoFactor);
        return new GrpcDecimal(units, nanos);
    }

    public static explicit operator GrpcDecimal(float value)
    {

        var units = (long)value;
        var nanos = (int)(value - units) * (int)NanoFactor;

        return new GrpcDecimal(units, nanos);
    }

    public static implicit operator NullableGrpcDecimal(GrpcDecimal grpcDecimal)
        => new NullableGrpcDecimal(grpcDecimal.Units, grpcDecimal.Nanos);

    public static explicit operator GrpcDecimal(NullableGrpcDecimal nullableGrpcDecimal) => new(nullableGrpcDecimal.Units, nullableGrpcDecimal.Nanos);
}
