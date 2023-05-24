namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

internal class CustomNumberFormatter : IAcroFieldFormatter
{
    private CustomNumberFormatter()
    {
    }

    public static IAcroFieldFormatter Instance { get; } = new CustomNumberFormatter();

    public string Format(object obj, IFormatProvider formatProvider)
    {
        if (obj is not decimal && obj is not int)
            throw new ArgumentException("Decimal or integer was expected.");

        var decimalValue = ((IConvertible)obj).ToDecimal(formatProvider);

        return decimalValue.ToString("{0:#,0.##}", formatProvider);
    }
}