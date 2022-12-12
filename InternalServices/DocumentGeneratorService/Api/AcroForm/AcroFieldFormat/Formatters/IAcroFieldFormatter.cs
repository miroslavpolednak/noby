namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat.Formatters;

internal interface IAcroFieldFormatter
{
    string Format(object obj, IFormatProvider formatProvider);
}