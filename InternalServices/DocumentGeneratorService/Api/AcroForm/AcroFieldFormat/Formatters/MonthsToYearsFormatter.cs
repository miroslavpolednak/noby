namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

public class MonthsToYearsFormatter : IAcroFieldFormatter
{
    private MonthsToYearsFormatter()
    {
    }

    public static MonthsToYearsFormatter Instance { get; } = new();

    public string Format(object obj, IFormatProvider formatProvider)
    {
        if (obj is not int number)
            throw new ArgumentException("Integer was expected", nameof(obj));

        var years = number / 12;
        var months = number % 12;

        var formattableString = (months == 0)
            ? (FormattableString)$"{years} {GetYearsText(years)}"
            : $"{years} {GetYearsText(years)} a {months} {GetMonthsText(months)}";

        return formattableString.ToString(formatProvider);
    }

    private static string GetYearsText(int numberOfYears) =>
        numberOfYears switch
        {
            1 => "rok",
            >= 2 and <= 4 => "roky",
            >= 5 => "let",
            _ => throw new ArgumentOutOfRangeException(nameof(numberOfYears), numberOfYears, null)
        };

    private static string GetMonthsText(int numberOfMonths) =>
        numberOfMonths switch
        {
            1 => "měsíc",
            >= 2 and <= 4 => "měsíce",
            >= 5 => "měsíců",
            _ => throw new ArgumentOutOfRangeException(nameof(numberOfMonths), numberOfMonths, null)
        };
}