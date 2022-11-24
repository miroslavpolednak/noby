namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class GrpcDate
{
    public GrpcDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static implicit operator DateOnly(GrpcDate grpcDate)
    {
        if (grpcDate == null)
            throw new ArgumentNullException(nameof(grpcDate));
        return new DateOnly(grpcDate.Year, grpcDate.Month, grpcDate.Day);
    }

    public static implicit operator GrpcDate(DateOnly value)
    {
        return new GrpcDate(value.Year, value.Month, value.Day);
    }

    public static implicit operator DateTime(GrpcDate grpcDate)
        => new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);

    public static implicit operator DateTime?(GrpcDate grpcDate)
        => grpcDate == null ? null : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);

    public static implicit operator GrpcDate(DateTime value)
        => new GrpcDate(value.Year, value.Month, value.Day);

    public static explicit operator GrpcDate(NullableGrpcDate nullableGrpcDate) => new(nullableGrpcDate.Year, nullableGrpcDate.Month, nullableGrpcDate.Day);
}
