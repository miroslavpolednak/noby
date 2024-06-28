namespace SharedTypes.GrpcTypes;

public partial class NullableGrpcDate
{
    public NullableGrpcDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static implicit operator DateOnly?(NullableGrpcDate? grpcDate)
    {
        if (grpcDate == null) return null;

        return new DateOnly(grpcDate.Year, grpcDate.Month, grpcDate.Day);
    }

    public static implicit operator NullableGrpcDate?(DateOnly? value)
    {
        if (value == null) return null;

        return new NullableGrpcDate(value.Value.Year, value.Value.Month, value.Value.Day);
    }

    public static implicit operator NullableGrpcDate?(DateTime? value)
    {
        if (value == null) return null;

        return new NullableGrpcDate(value.Value.Year, value.Value.Month, value.Value.Day);
    }

    public static implicit operator DateTime?(NullableGrpcDate? grpcDate)
        => grpcDate == null ? null : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);

    public static implicit operator DateTime(NullableGrpcDate grpcDate)
        => grpcDate == null ? default : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);
}
