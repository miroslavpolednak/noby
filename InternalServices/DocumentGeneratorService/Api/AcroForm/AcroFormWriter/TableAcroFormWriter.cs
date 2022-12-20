using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

public class TableAcroFormWriter : IAcroFormWriter
{
    private const string TemplateFirstPageSuffix = "S1";
    private const string TemplateTablePageSuffix = "SN";

    private readonly string _tablePlaceholderKey;
    private readonly IAcroFormWriter _acroFormWriter;
    private readonly PdfTable _table;

    public TableAcroFormWriter(AcroFieldFormatProvider fieldFormatProvider, GenerateDocumentPartData tableData, IAcroFormWriter acroFormWriter)
    {
        _tablePlaceholderKey = tableData.Key;
        _acroFormWriter = acroFormWriter;
        _table = new PdfTable(fieldFormatProvider, tableData.Table);
    }

    public MergeDocument Write(TemplateLoader templateLoader, string? templateNameModifier = default)
    {
        var loadedData = InitializeDocument(templateLoader);

        DrawRemainingRows(templateLoader, loadedData);

        _table.DrawConcludingParagraph(loadedData.MergeDocument.Pages[^1],
                                       loadedData.PlaceholderField,
                                       () => AppendPage(loadedData, GetRootFormFieldName(loadedData.MergeDocument)));

        return loadedData.MergeDocument;
    }

    private LoadedTemplateData InitializeDocument(TemplateLoader templateLoader)
    {
        var loadedData = new LoadedTemplateData
        {
            Template = templateLoader.Load(TemplateFirstPageSuffix),
            MergeDocument = _acroFormWriter.Write(templateLoader, TemplateFirstPageSuffix),
        };

        loadedData.PlaceholderField = loadedData.Template.Form.Fields[_tablePlaceholderKey];

        var placeholderField = loadedData.Template.Form.Fields[_tablePlaceholderKey];

        if (placeholderField is null)
            throw new InvalidOperationException($"Placeholder field {_tablePlaceholderKey} was not found.");

        var page = loadedData.MergeDocument.Pages[placeholderField.GetOriginalPageNumber() - 1];

        _table.DrawTable(page, placeholderField);

        loadedData.MergeDocument.Form.Fields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;

        return loadedData;
    }

    private void DrawRemainingRows(TemplateLoader templateLoader, LoadedTemplateData loadedData)
    {
        if (!_table.HasOverflowRows)
            return;

        loadedData.Template = templateLoader.Load(TemplateTablePageSuffix);
        loadedData.PlaceholderField = loadedData.Template.Form.Fields[_tablePlaceholderKey];

        while (_table.HasOverflowRows)
        {
            var rootFormFieldName = GetRootFormFieldName(loadedData.MergeDocument);

            var page = AppendPage(loadedData, rootFormFieldName);

            _table.DrawOverflowRows(page, loadedData.PlaceholderField);

            loadedData.MergeDocument.Form.Fields[rootFormFieldName].ChildFields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;
        }
    }

    private static string GetRootFormFieldName(Document document) => (document.Pages.Count + 1).ToString();

    private static AppendedPage AppendPage(LoadedTemplateData loadedData, string rootFormFieldName)
    {
        var mergeOptions = MergeOptions.Append;
        mergeOptions.RootFormField = rootFormFieldName;

        return loadedData.MergeDocument.Append(loadedData.Template, 1, 1, mergeOptions).First();
    }

    private class LoadedTemplateData
    {
        public required PdfDocument Template { get; set; }

        public required MergeDocument MergeDocument { get; init; }

        public Pdf.Merger.Forms.PdfFormField PlaceholderField { get; set; } = null!;
    }
}