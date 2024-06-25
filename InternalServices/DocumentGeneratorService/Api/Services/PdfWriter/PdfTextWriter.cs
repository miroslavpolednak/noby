using System.ComponentModel;
using ceTe.DynamicPDF.PageElements;
using CIS.Core.Exceptions;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Extensions;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;
namespace CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfWriter;

public class PdfTextWriter : IPdfWriter
{
    private readonly AcroFieldFormatProvider _formatProvider;

    public PdfTextWriter(AcroFieldFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public MergeDocument WriteToDocument(PdfDocument pdfDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, ICollection<GenerateDocumentPartData> values)
    {
        var mergeDocument = new MergeDocument(pdfDocument)
        {
            PdfVersion = PdfVersion.v1_7
        };

        foreach (var value in FilterValues(values))
        {
            var documentMapItem = fieldMap.GetValueOrDefault(value.Key) ?? throw new CisValidationException(400, $"Unknown key {value.Key} for selected template.");

            if (documentMapItem.HasChildFields)
            {
                documentMapItem.ChildFields!.ForEach(childField => AddTextField(childField, value));
            }
            else
            {
                AddTextField(documentMapItem.Field, value);
            }
        }

        return mergeDocument;

        void AddTextField(FieldInfo fieldInfo, GenerateDocumentPartData value) => mergeDocument.Pages[fieldInfo.PageNumber - 1].Elements.Add(CreateTextField(fieldInfo, value));
    }

    private TextArea CreateTextField(FieldInfo fieldInfo, GenerateDocumentPartData fieldData)
    {
        return new TextArea(GetText(fieldData),
                            fieldInfo.Coordinates.XWithOffset(),
                            fieldInfo.Coordinates.YWithOffset(),
                            fieldInfo.Coordinates.Width,
                            fieldInfo.Coordinates.Height,
                            FontHelper.ParseOpenTypeFont(fieldInfo.FontName),
                            fieldInfo.FontSize)
        {
            Align = fieldData.TextAlign == Contracts.TextAlign.Unkwnon ? Pdf.TextAlign.Left : (Pdf.TextAlign)fieldData.TextAlign,
            VAlign = fieldData.VAlign == Contracts.VAlign.Unknown ? Pdf.VAlign.Top : (Pdf.VAlign)fieldData.VAlign
        };
    }

    private static IEnumerable<GenerateDocumentPartData> FilterValues(ICollection<GenerateDocumentPartData> values) => 
        values.Where(v => v.ValueCase != GenerateDocumentPartData.ValueOneofCase.Table);

    private string GetText(GenerateDocumentPartData value)
    {
        return value.ValueCase switch
        {
            GenerateDocumentPartData.ValueOneofCase.None => value.StringFormat ?? string.Empty,
            GenerateDocumentPartData.ValueOneofCase.Text => GetFormattedString(value.Text, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.Date => GetFormattedString<DateTime>(value.Date, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.Number => GetFormattedString(value.Number, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.DecimalNumber => GetFormattedString<decimal>(value.DecimalNumber, value.StringFormat),
            GenerateDocumentPartData.ValueOneofCase.LogicalValue => GetFormattedString(value.LogicalValue, value.StringFormat),
            _ => throw new InvalidEnumArgumentException(nameof(value.ValueCase), (int)value.ValueCase, typeof(GenerateDocumentPartData.ValueOneofCase))
        };
    }

    private string GetFormattedString(string text, string? format) => format is null ? text : string.Format(_formatProvider, format, text);

    private static string GetFormattedString(bool boolean, string? format) => format ?? (boolean ? "Yes" : "No");

    private string GetFormattedString<TValue>(TValue value, string? format) where TValue : notnull => _formatProvider.Format(value, format);
}