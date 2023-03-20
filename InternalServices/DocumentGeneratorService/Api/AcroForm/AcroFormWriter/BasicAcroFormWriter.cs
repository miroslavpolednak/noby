﻿using CIS.Core.Exceptions;
using System.ComponentModel;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;

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
        var document = new MergeDocument(pdfDocument);

        foreach (var value in _values)
        {
            var field = document.Form.Fields[value.Key];

            if (field is null)
                throw new CisArgumentException(400, $"Unknown key {value.Key} for selected template.", nameof(value.Key));

            field.Value = GetFieldValue(value);
        }

        return document;
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

    private static string GetFormattedString(string text, string? format) => format is null ? text : string.Format(format, text);

    private static string GetFormattedString(bool boolean, string? format) => format ?? (boolean ? "Yes" : "No");

    private string GetFormattedString<TValue>(TValue value, string? format) where TValue : notnull => _fieldFormatProvider.Format(value, format);
}