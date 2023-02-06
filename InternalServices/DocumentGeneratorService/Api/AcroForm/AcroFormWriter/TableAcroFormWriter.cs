using ceTe.DynamicPDF.Merger.Forms;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

public class TableAcroFormWriter : IAcroFormWriter
{
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

        DrawTitlePage(loadedData);
        DrawRemainingRows(loadedData);

        _table.DrawConcludingParagraph(loadedData.GetPage(^1), loadedData.GetPlaceholderSubFiled(^1), () => AppendPage(loadedData));

        loadedData.MergeDocument.Form.Fields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;

        return loadedData.MergeDocument;
    }

    private LoadedTemplateData InitializeDocument(TemplateLoader templateLoader)
    {
        var loadedData = new LoadedTemplateData
        {
            Template = templateLoader.Load(),
            MergeDocument = _acroFormWriter.Write(templateLoader)
        };

        loadedData.PlaceholderField = loadedData.Template.Form.Fields[_tablePlaceholderKey];

        if (loadedData.PlaceholderField is null)
            throw new InvalidOperationException($"Placeholder field {_tablePlaceholderKey} was not found.");

        return loadedData;
    }

    private void DrawTitlePage(LoadedTemplateData loadedTemplateData)
    {
        var firstPageNumber = loadedTemplateData.GetPlaceholderSubFiled(0).GetOriginalPageNumber() - 1;

        _table.DrawTable(loadedTemplateData.GetPage(firstPageNumber), loadedTemplateData.GetPlaceholderSubFiled(0));

        if (_table.HasOverflowRows)
        {
            _table.DrawOverflowRows(loadedTemplateData.GetPage(firstPageNumber + 1), loadedTemplateData.GetPlaceholderSubFiled(^1));

            return;
        }

        loadedTemplateData.MergeDocument = new MergeDocument(loadedTemplateData.Template, 1, loadedTemplateData.Template.Pages.Count - 2);
    }

    private void DrawRemainingRows(LoadedTemplateData loadedData)
    {
        while (_table.HasOverflowRows)
        {
            var rootFormFieldName = loadedData.GetRootFormFieldName();

            var page = AppendPage(loadedData);

            _table.DrawOverflowRows(page, loadedData.GetPlaceholderSubFiled(^1));

            loadedData.MergeDocument.Form.Fields[rootFormFieldName].ChildFields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;
        }
    }

    private static AppendedPage AppendPage(LoadedTemplateData loadedData)
    {
        var mergeOptions = MergeOptions.Append;
        mergeOptions.RootFormField = loadedData.GetRootFormFieldName();

        return loadedData.MergeDocument.Append(loadedData.Template, loadedData.Template.Pages.Count, 1, mergeOptions).First();
    }

    private class LoadedTemplateData
    {
        public required PdfDocument Template { get; init; }

        public required MergeDocument MergeDocument { get; set; }

        public PdfFormField PlaceholderField { get; set; } = null!;

        public Page GetPage(Index pageIndex) => MergeDocument.Pages[pageIndex];

        public PdfFormField GetPlaceholderSubFiled(Index index) => PlaceholderField.ChildFields[index];

        public string GetRootFormFieldName() => (MergeDocument.Pages.Count + 1).ToString();
    }
}