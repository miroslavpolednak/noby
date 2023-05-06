using System.Globalization;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

internal class CurrencyFormatter : IAcroFieldFormatter
{
    private CurrencyFormatter()
    {
    }

    public static IAcroFieldFormatter Instance { get; } = new CurrencyFormatter();

    public string Format(object obj, IFormatProvider formatProvider)
    {
        if (obj is not decimal && obj is not int)
            throw new ArgumentException("Decimal was expected.");

        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);

        return string.Format(numberFormatInfo, "{0:#,0.##}" + $" {numberFormatInfo.CurrencySymbol}", (decimal)obj);
    }
}