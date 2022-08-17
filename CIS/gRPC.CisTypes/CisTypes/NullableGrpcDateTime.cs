namespace CIS.Infrastructure.gRPC.CisTypes;

public sealed partial class NullableGrpcDateTime
{
    public NullableGrpcDateTime(int year, int month, int day, int hours, int minutes, int seconds, int nanos)
    {
        Year = year;
        Month = month;
        Day = day;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
        Nanos = nanos;
    }

    public static implicit operator NullableGrpcDateTime?(DateTime? value)
    {
        if (value == null) return default(NullableGrpcDateTime);
        return new NullableGrpcDateTime(value.Value.Year, value.Value.Month, value.Value.Day, value.Value.Hour, value.Value.Minute, value.Value.Second, value.Value.Millisecond);
    }

    public static implicit operator NullableGrpcDateTime(DateTime value)
    {
        return new NullableGrpcDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
    }

    public static implicit operator DateTime?(NullableGrpcDateTime? grpcDate)
        => grpcDate == null ? default(DateTime?) : new DateTime(grpcDate.Year, grpcDate.Month, grpcDate.Day, grpcDate.Hours, grpcDate.Minutes, grpcDate.Seconds, grpcDate.Nanos);
}
