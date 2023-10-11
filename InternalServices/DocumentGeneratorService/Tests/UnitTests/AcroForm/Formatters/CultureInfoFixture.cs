using System.Globalization;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Formatters;

public class CultureInfoFixture
{
    public static CultureInfo CultureInfo
    {
        get
        {
            var cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("cs").Clone();

            cultureInfo.NumberFormat.CurrencySymbol = "Kč";

            return cultureInfo;
        }
    }
}