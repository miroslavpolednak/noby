using System.Globalization;

namespace DomainServices.SalesArrangementService.Api;
internal static class JsonExtensions
{
    private static readonly string FormatDecimal = "###########0.00";
    private static readonly string FormatDate = "dd.MM.yyyy";

    public static string ToJsonString(this CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal value)
    {
        return ((decimal)value).ToString(FormatDecimal, CultureInfo.InvariantCulture);
    }

    public static string ToJsonString(this int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToJsonString(this decimal value)
    {
        return value.ToString(FormatDecimal, CultureInfo.InvariantCulture);
    }

    public static string? ToJsonString(this int? value)
    {
        return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
    }

    public static string ToJsonString(this DateTime value)
    {
        return value.ToString(FormatDate, CultureInfo.InvariantCulture);
    }
    public static string ToJsonString(this bool value)
    {
        return value ? "1" : "0";
    }

}
