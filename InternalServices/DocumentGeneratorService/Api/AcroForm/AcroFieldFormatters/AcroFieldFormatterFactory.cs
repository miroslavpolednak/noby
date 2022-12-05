namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormatters;

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
            { CustomFormatterKeys.Percentage, PercentageFormatter.Instance }
        };
}