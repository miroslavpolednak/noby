using System.Globalization;

namespace DomainServices.SalesArrangementService.Api;
internal static class JsonExtensions
{
    public static string ToJsonString(this CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal value)
    {
        return ((decimal)value).ToString(CultureInfo.InvariantCulture);
    }

    public static string ToJsonString(this int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static string? ToJsonString(this int? value)
    {
        return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
    }

}
