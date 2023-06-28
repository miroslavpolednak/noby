using CIS.Infrastructure.gRPC.CisTypes;

namespace DomainServices.CaseService.Api;

internal static class TaskDataExtensions
{
    private static CultureInfo _czCulture = new CultureInfo("cs-CZ");

    public static decimal GetDecimal(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        if (decimal.TryParse(taskData[key], CultureInfo.InvariantCulture, out decimal d))
        {
            return d;
        }
        else
        {
            return decimal.Parse(taskData[key], _czCulture);
        }
    }

    public static int GetInteger(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return int.Parse(taskData[key], CultureInfo.InvariantCulture);
    }

    public static int? GetNInteger(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return int.TryParse(taskData.GetValueOrDefault(key), out int v) ? v : null;
    }

    public static long GetLong(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return long.Parse(taskData[key], CultureInfo.InvariantCulture);
    }

    public static DateTime? GetDate(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        if (DateTime.TryParse(taskData[key], _czCulture, out DateTime d2))
        {
            return d2;
        }
        else if (DateTime.TryParse(taskData[key], CultureInfo.InvariantCulture, out DateTime d1))
        {
            return d1;
        }
        return null;
    }

    public static bool GetBoolean(this IReadOnlyDictionary<string, string> taskData, string key)
    {
        return Convert.ToBoolean(int.Parse(taskData[key], CultureInfo.InvariantCulture));
    }

    public static string ToSbFormat(this DateOnly date)
        => date.ToString(_czCulture);

    public static string ToSbFormat(this decimal d)
        => d.ToString(CultureInfo.InvariantCulture);

    public static string ToSbFormat(this decimal? d)
        => d.HasValue ? d.Value.ToString(CultureInfo.InvariantCulture) : "";

    public static string ToSbFormat(this GrpcDecimal d)
        => ((decimal)d).ToString(CultureInfo.InvariantCulture);

    public static string ToSbFormat(this NullableGrpcDecimal d)
        => d is null ? "" : ((decimal)d!).ToString(CultureInfo.InvariantCulture);
}