﻿using System.Globalization;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

internal class CurrencyFormatter : IAcroFieldFormatter
{
    private CurrencyFormatter()
    {
    }

    public static IAcroFieldFormatter Instance { get; } = new CurrencyFormatter();

    public string Format(object obj, IFormatProvider formatProvider)
    {
        if (obj is not decimal decimalNumber)
            throw new ArgumentException("Decimal was expected.");

        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);

        return decimalNumber.ToString($"0.## {numberFormatInfo.CurrencySymbol}", numberFormatInfo);
    }
}