namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroFieldFormatters;

internal class PercentageFormatter : IAcroFieldFormatter
{
    private PercentageFormatter()
    {
    }

    public static IAcroFieldFormatter Instance { get; } = new PercentageFormatter();

    public string Format(object obj, IFormatProvider formatProvider)
    {
        if (obj is not decimal decimalNumber)
            throw new ArgumentException("Decimal was expected.");

        return (decimalNumber / 100).ToString("P2", formatProvider);
    }
}