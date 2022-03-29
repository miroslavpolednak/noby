using System.Globalization;

namespace DomainServices.SalesArrangementService.Api;
internal static class JsonExtensions
{
    private static readonly string FormatDecimal = "###########0.00";
    private static readonly string FormatDate = "dd.MM.yyyy";


    public static decimal? ToDecimal(this CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal value)
    {
        return  value == null ? null : (decimal)value;
    }

    public static string? ToJsonString(this CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal value)
    {
        return value.ToDecimal().ToJsonString();
    }

    public static string ToJsonString(this int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static string? ToJsonString(this int? value)
    {
        return value.HasValue ? value.Value.ToJsonString() : null;
    }

    public static string ToJsonString(this decimal value)
    {
        return value.ToString(FormatDecimal, CultureInfo.InvariantCulture);
    }

    public static string? ToJsonString(this decimal? value)
    {
        return value.HasValue ? value.Value.ToJsonString() : null;
    }

    public static string ToJsonString(this DateTime value)
    {
        return value.ToString(FormatDate, CultureInfo.InvariantCulture);
    }
    public static string? ToJsonString(this DateTime? value)
    {
        return value.HasValue ? value.Value.ToJsonString() : null;
    }

    public static string? ToJsonString(this CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDate value)
    {
        return ((DateTime?)value).ToJsonString();
    }

    public static string ToJsonString(this bool value)
    {
        return value ? "1" : "0";
    }

    public static string ToCode(this CIS.Infrastructure.gRPC.CisTypes.Identity identity)
    {
        return $"{identity.IdentityScheme}|{identity.IdentityId}";
    }

}
