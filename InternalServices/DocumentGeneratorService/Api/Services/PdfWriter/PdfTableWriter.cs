using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;
using CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfElements;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfWriter;

public class PdfTableWriter : IPdfWriter
{
    private readonly AcroFieldFormatProvider _formatProvider;
    private readonly PdfTextWriter _pdfTextWriter;

    public PdfTableWriter(AcroFieldFormatProvider formatProvider, PdfTextWriter pdfTextWriter)
    {
        _formatProvider = formatProvider;
        _pdfTextWriter = pdfTextWriter;
    }

    public MergeDocument WriteToDocument(PdfDocument pdfDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, ICollection<GenerateDocumentPartData> values)
    {
        var tableData = GetTableData(values);
        var tableFieldInfo = GetTableFieldInfo(fieldMap, tableData);

        var pdfTable = new PdfTable(_formatProvider, tableData.Table);

        var mergeDocument = _pdfTextWriter.WriteToDocument(pdfDocument, fieldMap, values);
        mergeDocument = DrawTitlePage(pdfDocument, mergeDocument, pdfTable, tableFieldInfo);
        DrawRemainingRows(pdfDocument, mergeDocument, pdfTable, tableFieldInfo.ChildFields![^1]);

        pdfTable.DrawConcludingParagraph(mergeDocument.Pages[^1], tableFieldInfo.ChildFields![^1], () => AppendPage(pdfDocument, mergeDocument));

        return mergeDocument;
    }

    private static MergeDocument DrawTitlePage(PdfDocument pdfDocument, MergeDocument mergeDocument, PdfTable pdfTable, DocumentMapItem tableFieldInfo)
    {
        pdfTable.DrawTable(mergeDocument.Pages[tableFieldInfo.Field.PageNumber - 1], tableFieldInfo.Field);

        if (pdfTable.HasOverflowRows)
        {
            pdfTable.DrawOverflowRows(mergeDocument.Pages[tableFieldInfo.ChildFields![^1].PageNumber - 1], tableFieldInfo.ChildFields[^1]);

            return mergeDocument;
        }

        return new MergeDocument(pdfDocument, 1, pdfDocument.Pages.Count - 2);
    }

    private void DrawRemainingRows(PdfDocument pdfDocument, MergeDocument mergeDocument, PdfTable pdfTable, FieldInfo fieldInfo)
    {
        while (pdfTable.HasOverflowRows)
        {
            var page = AppendPage(pdfDocument, mergeDocument);

            pdfTable.DrawOverflowRows(page, fieldInfo);
        }
    }

    private static AppendedPage AppendPage(PdfDocument pdfDocument, MergeDocument mergeDocument)
    {
        var mergeOptions = MergeOptions.Append;

        return mergeDocument.Append(pdfDocument, pdfDocument.Pages.Count, 1, mergeOptions).First();
    }

    private static GenerateDocumentPartData GetTableData(ICollection<GenerateDocumentPartData> values)
    {
        var tables = values.Where(v => v.ValueCase == GenerateDocumentPartData.ValueOneofCase.Table).ToList();

        if (tables.Count > 1)
            throw new InvalidOperationException("Multiple tables are not supported.");

        return tables.First();
    }

    private static DocumentMapItem GetTableFieldInfo(IReadOnlyDictionary<string, DocumentMapItem> fieldMap, GenerateDocumentPartData tableData)
    {
        var documentMapItem = fieldMap.GetValueOrDefault(tableData.Key) ?? throw new InvalidOperationException($"Placeholder field {tableData.Key} was not found.");;

        return documentMapItem;
    }
}