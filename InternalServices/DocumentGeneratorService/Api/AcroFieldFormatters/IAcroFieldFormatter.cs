namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroFieldFormatters;

internal interface IAcroFieldFormatter
{
    string Format(object obj, IFormatProvider formatProvider);
}