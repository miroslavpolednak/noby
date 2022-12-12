namespace CIS.Infrastructure.gRPC.CisTypes;

public partial class GrpcDate
{
    public GrpcDate(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public static implicit operator DateOnly(GrpcDate grpcDate)
    {
        ArgumentNullException.ThrowIfNull(grpcDate);

        return new DateOnly(grpcDate.Year, grpcDate.Month, grpcDate.Day);
    }

    public static implicit operator GrpcDate(DateOnly value)
        => new GrpcDate(value.Year, value.Month, value.Day);

    public static implicit operator DateTime(GrpcDate grpcDate)
        => new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);

    public static implicit operator DateTime?(GrpcDate grpcDate)
        => grpcDate == null ? null : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day);

    public static implicit operator GrpcDate(DateTime value)
        => new GrpcDate(value.Year, value.Month, value.Day);

    public static explicit operator GrpcDate(NullableGrpcDate nullableGrpcDate) => new(nullableGrpcDate.Year, nullableGrpcDate.Month, nullableGrpcDate.Day);
}
