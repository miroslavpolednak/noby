namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class NullableGrpcDate
{
    public NullableGrpcDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static implicit operator DateOnly?(NullableGrpcDate grpcDate)
    {
        if (grpcDate == null) return default(DateOnly?);
        return new DateOnly(grpcDate.Year, grpcDate.Month, grpcDate.Day);
    }

    public static implicit operator NullableGrpcDate?(DateOnly? value)
    {
        if (value == null) return default(NullableGrpcDate);
        return new NullableGrpcDate(value.Value.Year, value.Value.Month, value.Value.Day);
    }

    public static implicit operator NullableGrpcDate?(DateTime? value)
    {
        if (value == null) return default(NullableGrpcDate);
        return new NullableGrpcDate(value.Value.Year, value.Value.Month, value.Value.Day);
    }

    public static implicit operator DateTime?(NullableGrpcDate? grpcDate)
        => grpcDate == null ? default(DateTime?) : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);
}
