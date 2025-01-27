﻿using CIS.Core.Exceptions;
using System.ComponentModel;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Extensions;
using TextAlign = CIS.InternalServices.DocumentGeneratorService.Contracts.TextAlign;
using VAlign = CIS.InternalServices.DocumentGeneratorService.Contracts.VAlign;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

public class BasicAcroFormWriter : IAcroFormWriter
{
    private readonly AcroFieldFormatProvider _fieldFormatProvider;
    private readonly IEnumerable<GenerateDocumentPartData> _values;

    public BasicAcroFormWriter(AcroFieldFormatProvider fieldFormatProvider, IEnumerable<GenerateDocumentPartData> values)
    {
        _fieldFormatProvider = fieldFormatProvider;
        _values = values;
    }

    public MergeDocument Write(PdfDocument pdfDocument, string? templateNameModifier = default)
    {
        var document = new MergeDocument(pdfDocument)
        {
            PdfVersion = PdfVersion.v1_7
        };

        foreach (var value in _values)
        {
            var field = document.Form.Fields[value.Key] ?? throw new CisValidationException(400, $"Unknown key {value.Key} for selected template.");

            field.Font = field.Font.ParseOpenTypeFont();

            if (value.TextAlign != TextAlign.Unkwnon || value.VAlign != VAlign.Unknown)
            {
                CreateAlignedText(pdfDocument, document, value);
                field.Output = FormFieldOutput.Remove;

                continue;
            }

            field.Value = GetFieldValue(value);
        }

        return document;
    }

    private void CreateAlignedText(PdfDocument pdfDocument, Document document, GenerateDocumentPartData data)
    {
        var pdfFormField = pdfDocument.Form.Fields[data.Key];

        try
        {
            var page = document.Pages[pdfFormField.GetOriginalPageNumber() - 1];

            var label = pdfFormField.CreateLabel(page, 0, 0, GetFieldValue(data), pdfFormField.Font.ParseOpenTypeFont(), pdfFormField.FontSize);
            label.Width -= 2;
            label.Align = data.TextAlign == TextAlign.Unkwnon ? Pdf.TextAlign.Left : (Pdf.TextAlign)data.TextAlign;
            label.VAlign = data.VAlign == VAlign.Unknown ? Pdf.VAlign.Top : (Pdf.VAlign)data.VAlign;
        }
        catch (Exception ex) when (ex is IndexOutOfRangeException or ArgumentOutOfRangeException or NullReferenceException)
        {
            throw new CisValidationException($"AcroField {data.Key} has incorrect formatting (probably multiple Acrofields with the same name or something similar)");
        }
    }

    private string GetFieldValue(GenerateDocumentPartData value)
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

    private string GetFormattedString(string text, string? format) => format is null ? text : string.Format(_fieldFormatProvider, format, text);

    private static string GetFormattedString(bool boolean, string? format) => format ?? (boolean ? "Yes" : "No");

    private string GetFormattedString<TValue>(TValue value, string? format) where TValue : notnull => _fieldFormatProvider.Format(value, format);
}