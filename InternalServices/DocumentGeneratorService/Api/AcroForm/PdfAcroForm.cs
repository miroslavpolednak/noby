using System.ComponentModel;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Pdf = ceTe.DynamicPDF;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[ScopedService, SelfService]
public class PdfAcroForm
{
    private readonly AcroFieldFormatProvider _fieldFormatProvider;

    public PdfAcroForm(AcroFieldFormatProvider fieldFormatProvider)
    {
        _fieldFormatProvider = fieldFormatProvider;
    }

    public void Fill(Pdf.Document document, IEnumerable<GenerateDocumentPartData> values)
    {
        foreach (var value in values)
        {
            var field = document.Form.Fields[value.Key];

            if (field is null)
                throw new CisArgumentException(400, $"Unknown key {value.Key} for selected template.", nameof(value.Key));

            field.Value = GetFieldValue(value);
        }
    }

    private string GetFieldValue(GenerateDocumentPartData value)
    {
        return value.ValueCase switch
        {
            GenerateDocumentPartData.ValueOneofCase.None => string.Empty,
            GenerateDocumentPartData.ValueOneofCase.Text => GetFormattedString(value.Text, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.Date => GetFormattedString<DateTime>(value.Date, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.Number => GetFormattedString(value.Number, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.DecimalNumber => GetFormattedString<decimal>(value.DecimalNumber, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.LogicalValue => GetFormattedString(value.LogicalValue),
            _ => throw new InvalidEnumArgumentException(nameof(value.ValueCase), (int)value.ValueCase, typeof(GenerateDocumentPartData.ValueOneofCase))
        };
    }

    private static string GetFormattedString(string text, string format) => string.IsNullOrWhiteSpace(format) ? text : string.Format(format, text);

    private static string GetFormattedString(bool boolean) => boolean ? "Yes" : "No";

    private string GetFormattedString<TValue>(TValue value, string format) where TValue : notnull => _fieldFormatProvider.Format(value, format);
}