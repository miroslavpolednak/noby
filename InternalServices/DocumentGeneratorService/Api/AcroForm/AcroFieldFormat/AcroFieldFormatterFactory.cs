using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;

internal static class AcroFieldFormatterFactory
{
    private static readonly Dictionary<string, IAcroFieldFormatter> _map;

    static AcroFieldFormatterFactory()
    {
        _map = CreateMap();
    }

    public static bool ContainsFormatter(string format) => _map.ContainsKey(format);

    public static IAcroFieldFormatter GetFormatter(string format) => _map[format];

    private static Dictionary<string, IAcroFieldFormatter> CreateMap() =>
        new()
        {
            { CustomFormatterKeys.MonthsToYears, MonthsToYearsFormatter.Instance },
            { CustomFormatterKeys.Percentage, PercentageFormatter.Instance },
            { CustomFormatterKeys.Currency, CurrencyFormatter.Instance }
        };
}