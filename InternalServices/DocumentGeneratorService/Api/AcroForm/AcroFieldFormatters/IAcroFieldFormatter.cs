namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormatters;

internal interface IAcroFieldFormatter
{
    string Format(object obj, IFormatProvider formatProvider);
}