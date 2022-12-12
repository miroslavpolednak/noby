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
        var document = InitializeDocument(templateLoader);

        DrawRemainingRows(templateLoader, document);

        return document;
    }

    private MergeDocument InitializeDocument(TemplateLoader templateLoader)
    {
        var template = templateLoader.Load(TemplateFirstPageSuffix);
        var document = _acroFormWriter.Write(templateLoader, TemplateFirstPageSuffix);

        var placeholderField = template.Form.Fields[_tablePlaceholderKey];
        var page = document.Pages[placeholderField.GetOriginalPageNumber() - 1];

        _table.DrawTable(page, placeholderField);

        document.Form.Fields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;

        return document;
    }

    private void DrawRemainingRows(TemplateLoader templateLoader, MergeDocument document)
    {
        if (!_table.HasOverflowRows)
            return;

        var template = templateLoader.Load(TemplateTablePageSuffix);
        var placeholderField = template.Form.Fields[_tablePlaceholderKey];

        while (_table.HasOverflowRows)
        {
            var rootFormFieldName = (document.Pages.Count + 1).ToString();

            var mergeOptions = MergeOptions.Append;
            mergeOptions.RootFormField = rootFormFieldName;

            var page = document.Append(template, 1, 1, mergeOptions).First();
            
            _table.DrawOverflowRows(page, placeholderField);

            document.Form.Fields[rootFormFieldName].ChildFields[_tablePlaceholderKey].Output = FormFieldOutput.Remove;
        }
    }
}