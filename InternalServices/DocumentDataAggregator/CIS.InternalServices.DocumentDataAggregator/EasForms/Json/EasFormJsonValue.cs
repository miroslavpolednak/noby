using System.Globalization;
using CIS.Infrastructure.gRPC.CisTypes;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.Json;

internal class EasFormJsonValue : EasFormJsonObject
{
    private const string FormatDecimal = "###########0.00";
    private const string FormatDate = "dd.MM.yyyy";

    public required string DataFieldPath { get; init; }

    public override void Add(string[] propertyPath, string dataFieldPath)
    {
        throw new NotImplementedException();
    }

    public override object? GetJsonObject(object data) => GetValueAsString(MapperHelper.GetValue(data, DataFieldPath));

    private static string? GetValueAsString(object? obj) =>
        obj switch
        {
            GrpcDecimal grpcDecimal => Format((decimal?)grpcDecimal),
            NullableGrpcDecimal nullableGrpcDecimal => Format((decimal?)nullableGrpcDecimal),
            decimal decimalNumber => Format(decimalNumber),
            GrpcDate grpcDate => Format((DateTime?)grpcDate),
            NullableGrpcDate nullableGrpcDate => Format((DateTime?)nullableGrpcDate),
            DateTime dateTime => Format(dateTime),
            bool b => b ? "1" : "0",
            Identity identity => $"{identity.IdentityScheme}|{identity.IdentityId}",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            null => null,
            _ => obj.ToString()
        };

    private static string? Format(decimal? value) => value?.ToString(FormatDecimal, CultureInfo.GetCultureInfo("cs-CZ"));

    private static string? Format(DateTime? value) => value?.ToString(FormatDate, CultureInfo.InvariantCulture);
}