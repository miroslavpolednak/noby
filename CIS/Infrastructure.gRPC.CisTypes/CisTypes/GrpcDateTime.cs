namespace CIS.Infrastructure.gRPC.CisTypes;

//TODO budeme si hrat na UTC, resp. timezones?
public partial class GrpcDateTime
{
    public GrpcDateTime(int year, int month, int day, int hours, int minutes, int seconds, int nanos)
    {
        Year = year;
        Month = month;
        Day = day;
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
        Nanos = nanos;
    }

    public static implicit operator DateTime?(GrpcDateTime time)
    {
        if (time == null)
            return null;

        try
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hours, time.Minutes, time.Seconds, time.Nanos);
        }
        catch
        {
            return null;
        }
    }

    public static implicit operator DateTime(GrpcDateTime time)
    {
        try
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hours, time.Minutes, time.Seconds, time.Nanos);
        }
        catch
        {
            // tady nevim jestli vyhazovat vyjimku nebo radsi vratit null nebo default()?
            // chyba by tady nemela nikdy nastat - jedine pri testovani pres nejaky Postman atd.
            throw new ArgumentException("GrpcDateTime can not be converted to regular datetime", nameof(time));
        }
    }

    public static implicit operator GrpcDateTime(DateTime value)
        => new GrpcDateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);

    public static implicit operator NullableGrpcDate(GrpcDateTime value)
        => value;
}