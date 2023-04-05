using System.Globalization;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

public class CultureProvider
{
    public static IFormatProvider GetProvider()
    {
        var culture = (CultureInfo)CultureInfo.GetCultureInfo("cs-CZ").Clone();

        culture.NumberFormat.CurrencySymbol = "Kč";

        return culture;
    }
}