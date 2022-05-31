using System.Globalization;

namespace DomainServices.SalesArrangementService.Api;
internal static class JsonExtensions
{
    private static readonly string FormatDecimal = "###########0.00";
    private static readonly string FormatDate = "dd.MM.yyyy";

    public static decimal? ToDecimal(this CIS.Infrastructure.gRPC.CisTypes.GrpcDecimal value)
    {
        return value == null ? null : (decimal)value;
    }

    public static decimal? ToDecimal(this CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDecimal value)
    {
        return value == null ? null : (decimal)value!;
    }

    public static string? ToJsonString(this CIS.Infrastructure.gRPC.CisTypes.NullableGrpcDecimal value)
    {
        return ((decimal?)value).ToJsonString();
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

    public static string ToJsonString(this long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToJsonString(this decimal value)
    {
        return value.ToString(FormatDecimal, CultureInfo.GetCultureInfo("cs-CZ"));
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

    public static string? ToPostCodeJsonString(this string value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var valueWithoutSpaces = new string(value.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

        var isNumber = Int64.TryParse(valueWithoutSpaces, out var num);

        if (!isNumber)
        {
            throw new CisArgumentException(99999, $"PostCode value '{value}' isn't covertable to number.", nameof(value));  //TODO: ErrorCode
        }

        return valueWithoutSpaces;
    }
}
