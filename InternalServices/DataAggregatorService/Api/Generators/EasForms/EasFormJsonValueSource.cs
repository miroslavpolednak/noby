using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.JsonBuilder.ValueSource;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms;

internal class EasFormJsonValueSource : JsonValueSource<EasFormSourceField>
{
    private const string FormatDecimal = "###########0.00";
    private const string FormatDate = "dd.MM.yyyy";

    private EasFormJsonValueSource(EasFormSourceField sourceField) : base(sourceField)
    {
    }

    public static IJsonValueSource Create(EasFormSourceField sourceField) => new EasFormJsonValueSource(sourceField);

    public override object? ParseValue(object? value, object aggregatedData) =>
        value switch
        {
            GrpcDecimal grpcDecimal => Format((decimal?)grpcDecimal),
            NullableGrpcDecimal nullableGrpcDecimal => Format((decimal?)nullableGrpcDecimal),
            decimal decimalNumber => Format(decimalNumber),
            GrpcDate grpcDate => Format((DateTime?)grpcDate),
            NullableGrpcDate nullableGrpcDate => Format((DateTime?)nullableGrpcDate),
            DateTime dateTime => Format(dateTime),
            bool b => b ? "1" : "0",
            Identity identity => identity.IdentityId.ToString(CultureInfo.InvariantCulture),
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            null => null,
            _ => value.ToString()
        };

    private static string? Format(decimal? value) => value?.ToString(FormatDecimal, CultureInfo.GetCultureInfo("cs-CZ"));

    private static string? Format(DateTime? value) => value?.ToString(FormatDate, CultureInfo.InvariantCulture);
}