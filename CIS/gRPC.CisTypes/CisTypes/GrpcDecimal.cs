namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class GrpcDecimal
    : IConvertible
{
    private const decimal NanoFactor = 1_000_000_000;
    public GrpcDecimal(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
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

    public static implicit operator NullableGrpcDecimal(GrpcDecimal grpcDecimal)
        => new NullableGrpcDecimal(grpcDecimal.Units, grpcDecimal.Nanos);

    #region IConvertible Implementation
    static T ThrowNotSupported<T>()
    {
        var ex = ThrowNotSupported(typeof(T));
        return (T)ex;
    }

    static object ThrowNotSupported(Type type)
    {
        throw new InvalidCastException($"Converting type \"{typeof(GrpcDecimal)}\" to type \"{type}\" is not supported.");
    }

    TypeCode IConvertible.GetTypeCode()
    {
        return TypeCode.Object;
    }

    bool IConvertible.ToBoolean(IFormatProvider? provider) => ThrowNotSupported<bool>();
    char IConvertible.ToChar(IFormatProvider? provider) => ThrowNotSupported<char>();
    sbyte IConvertible.ToSByte(IFormatProvider? provider) => ThrowNotSupported<sbyte>();
    byte IConvertible.ToByte(IFormatProvider? provider) => ThrowNotSupported<byte>();
    short IConvertible.ToInt16(IFormatProvider? provider) => ThrowNotSupported<short>();
    ushort IConvertible.ToUInt16(IFormatProvider? provider) => ThrowNotSupported<ushort>();
    int IConvertible.ToInt32(IFormatProvider? provider) => Convert.ToInt32(this.Units);
    uint IConvertible.ToUInt32(IFormatProvider? provider) => ThrowNotSupported<uint>();
    long IConvertible.ToInt64(IFormatProvider? provider) => this.Units;
    ulong IConvertible.ToUInt64(IFormatProvider? provider) => ThrowNotSupported<ulong>();
    float IConvertible.ToSingle(IFormatProvider? provider) => ThrowNotSupported<float>();
    double IConvertible.ToDouble(IFormatProvider? provider) => Convert.ToDouble((decimal)this);
    decimal IConvertible.ToDecimal(IFormatProvider? provider) => (decimal)this!;
    DateTime IConvertible.ToDateTime(IFormatProvider? provider) => ThrowNotSupported<DateTime>();
    string IConvertible.ToString(IFormatProvider? provider) => ThrowNotSupported<string>();

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
    {
        if (conversionType == typeof(GrpcDecimal))
            return this;

        return ThrowNotSupported(conversionType);
    }
    #endregion
}
