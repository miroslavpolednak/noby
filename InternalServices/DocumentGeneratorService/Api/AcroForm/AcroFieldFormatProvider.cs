using System.Globalization;
using CIS.Core.Attributes;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroFieldFormatters;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[SingletonService, SelfService]
public class AcroFieldFormatProvider : IFormatProvider, ICustomFormatter
{
    private readonly CultureInfo _cultureInfo;

    public AcroFieldFormatProvider()
    {
        var test = (CultureInfo)CultureInfo.GetCultureInfo("cs").Clone();

        test.NumberFormat.CurrencySymbol = "Kč";

        _cultureInfo = test;
    }

    public string Format(object value, string? format) =>
        format is null ? FormatValueType(value) : string.Format(this, format, value);

    object? IFormatProvider.GetFormat(Type? formatType)
    {
        if (typeof(ICustomFormatter) == formatType)
            return this;

        if (typeof(NumberFormatInfo) == formatType)
            return _cultureInfo.NumberFormat;

        if (typeof(DateTimeFormatInfo) == formatType)
            return _cultureInfo.DateTimeFormat;

        return default;
    }

    string ICustomFormatter.Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrWhiteSpace(format) || arg is null)
            return FormatValueType(arg ?? string.Empty);

        if (AcroFieldFormatterFactory.ContainsFormatter(format))
        {
            var formatter = AcroFieldFormatterFactory.GetFormatter(format);
            return formatter.Format(arg, this);
        }

        if (arg is IFormattable formattable)
        {
            return formattable.ToString(format, _cultureInfo);
        }

        return arg.ToString() ?? string.Empty;
    }

    private string FormatValueType(object value) =>
        value switch
        {
            int number => number.ToString("N0", _cultureInfo),
            decimal decimalNumber => decimalNumber.ToString("N2", _cultureInfo),
            DateTime dateTime => dateTime.ToString("d", _cultureInfo),
            _ => value.ToString() ?? string.Empty
        };
}