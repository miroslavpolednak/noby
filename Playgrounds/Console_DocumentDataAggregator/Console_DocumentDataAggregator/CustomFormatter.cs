using System.Globalization;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroFieldFormatters;

namespace Console_DocumentDataAggregator;

public class CustomFormatter : IFormatProvider, ICustomFormatter
{
    public object? GetFormat(Type? formatType)
    {
        return typeof(ICustomFormatter) == formatType ? this : null;
    }

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (format == "MonthsToYears")
        {
            return MonthsToYearsFormatter.Instance.Format(arg!, CultureInfo.CurrentCulture);
        }

        if (arg is IFormattable formattable)
        {
            return formattable.ToString(format, formatProvider);
        }

        return arg?.ToString() ?? string.Empty;
    }
}