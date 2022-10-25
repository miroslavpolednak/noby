using System.ComponentModel;
using ceTe.DynamicPDF.Forms;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Attributes;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Pdf = ceTe.DynamicPDF;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[ScopedService, SelfService]
public class PdfAcroForm
{
    public void Fill(Pdf.Document document, IEnumerable<GenerateDocumentPartData> values)
    {
        foreach (var value in values)
        {
            var field = document.Form.Fields[value.Key];

            if (field is null)
                throw new CisArgumentException(400, $"Unknown key {value.Key} for selected template.", nameof(value.Key));

            WriteValue(field, value);
        }
    }

    private static void WriteValue(FormField field, GenerateDocumentPartData value)
    {
        switch (value.ValueCase)
        {
            case GenerateDocumentPartData.ValueOneofCase.None:
            case GenerateDocumentPartData.ValueOneofCase.Text:
                Write(field, value.Text);
                break;

            case GenerateDocumentPartData.ValueOneofCase.Date:
                Write(field, value.Date);
                break;

            case GenerateDocumentPartData.ValueOneofCase.Number:
                Write(field, value.Number);
                break;

            case GenerateDocumentPartData.ValueOneofCase.DecimalNumber:
                Write(field, (decimal)value.DecimalNumber);
                break;

            case GenerateDocumentPartData.ValueOneofCase.LogicalValue:
                Write(field, value.LogicalValue);
                break;

            case GenerateDocumentPartData.ValueOneofCase.Table:
                throw new NotImplementedException();

            default:
                throw new InvalidEnumArgumentException(nameof(value.ValueCase), (int)value.ValueCase, typeof(GenerateDocumentPartData.ValueOneofCase));
        }
    }

    private static void Write(FormField field, string text)
    {
        field.Value = text;
    }

    private static void Write(FormField field, DateTime date)
    {
        field.Value = date.ToString("d");
    }

    private static void Write(FormField field, int number)
    {
        field.Value = number.ToString();
    }

    private static void Write(FormField field, decimal decimalNumber)
    {
        field.Value = decimalNumber.ToString();
    }

    private static void Write(FormField field, bool boolean)
    {
        field.Value = boolean ? "Yes" : "No";
    }
}