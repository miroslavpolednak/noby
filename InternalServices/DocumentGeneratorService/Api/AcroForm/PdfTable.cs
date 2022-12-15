using ceTe.DynamicPDF.Merger.Forms;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using System.Drawing;
using ceTe.DynamicPDF.PageElements;
using System.ComponentModel;
using Font = ceTe.DynamicPDF.Font;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

public class PdfTable
{
    private readonly AcroFieldFormatProvider _fieldFormatProvider;
    private readonly GenericTable _tableData;

    private Table2? _table;

    public PdfTable(AcroFieldFormatProvider fieldFormatProvider, GenericTable tableData)
    {
        _fieldFormatProvider = fieldFormatProvider;
        _tableData = tableData;
    }

    public bool HasOverflowRows => _table?.HasOverflowRows() ?? false;

    public void DrawTable(Page page, PdfFormField tablePlaceholder)
    {
        var position = GetTablePosition(tablePlaceholder, page);
        var size = GetTableSize(tablePlaceholder);

        _table = new Table2(position.X, position.Y, size.Width, size.Height);

        PrepareTable(_table);

        page.Elements.Add(_table);
    }

    public void DrawOverflowRows(Page page, PdfFormField tablePlaceholder)
    {
        if (_table is null)
            throw new InvalidOperationException("Table is not initialized.");

        var position = GetTablePosition(tablePlaceholder, page);
        var size = GetTableSize(tablePlaceholder);

        _table = _table.GetOverflowRows();

        Test(_table, page);

        _table.X = position.X;
        _table.Y = position.Y;
        _table.Width = size.Width;
        _table.Height = size.Height;

        page.Elements.Add(_table);
    }

    private void Test(Table2 table, Page page)
    {
        const float textTopMargin = 5f;

        if (string.IsNullOrWhiteSpace(_tableData.ConcludingParagraph) || HasOverflowRows)
            return;

        var remainingHeight = table.Height - table.GetVisibleHeight();

        var textArea = new TextArea(_tableData.ConcludingParagraph, table.X, table.Y + table.GetVisibleHeight() + textTopMargin, table.Width, remainingHeight - textTopMargin)
        {
            Font = Font.LoadSystemFont(new System.Drawing.Font("Arial", 10)),
            FontSize = 10,
            Align = TextAlign.Justify
        };

        var test = textArea.HasOverflowText();

        page.Elements.Add(textArea);
    }

    private static PointF GetTablePosition(PdfFormField pdfFormField, Page page) => new(pdfFormField.GetX(page), pdfFormField.GetY(page));

    private static SizeF GetTableSize(PdfFormField pdfFormField) => new(pdfFormField.Width, pdfFormField.Height);

    private void PrepareTable(Table2 table)
    {
        ApplyTableStyle(table);
        CreateColumns(table);
        CreateRows(table);
    }

    private static void ApplyTableStyle(Table2 table)
    {
        table.Border.Width = 0f;
        table.CellDefault.Align = TextAlign.Center;
        table.CellDefault.VAlign = VAlign.Center;
        table.CellDefault.Font = Pdf.Font.Helvetica;
        table.CellDefault.FontSize = 10;
        table.CellDefault.Border.Width = 0.5f;
    }

    private void CreateColumns(Table2 table)
    {
        var row = table.Rows.Add(20, Pdf.Font.HelveticaBold, table.CellDefault.FontSize, RgbColor.Black, RgbColor.LightGrey);
        
        foreach (var column in _tableData.Columns)
        {
            table.Columns.Add((float)(column.WidthPercentage / 100) * table.Width);

            row.Cells.Add(column.Header);
        }

        table.RepeatColumnHeaderCount = 1;
    }

    private void CreateRows(Table2 table)
    {
        foreach (var rowData in _tableData.Rows)
        {
            var row = table.Rows.Add(18);

            for (var index = 0; index < rowData.Values.Count; index++)
            {
                var stringValue = GetFormattedValue(rowData.Values[index], _tableData.Columns[index].StringFormat);

                row.Cells.Add(stringValue);
            }
        }
    }

    private string GetFormattedValue(GenericTableRowValue value, string? format)
    {
        return value.ValueCase switch
        {
            GenericTableRowValue.ValueOneofCase.None => format ?? string.Empty,
            GenericTableRowValue.ValueOneofCase.Text => format is null ? value.Text : string.Format(format, value.Text),
            GenericTableRowValue.ValueOneofCase.Date => GetFormattedString<DateTime>(value.Date),
            GenericTableRowValue.ValueOneofCase.Number => GetFormattedString(value.Number),
            GenericTableRowValue.ValueOneofCase.DecimalNumber => GetFormattedString<decimal>(value.DecimalNumber),
            _ => throw new InvalidEnumArgumentException(nameof(value.ValueCase), (int)value.ValueCase, typeof(GenericTableRowValue.ValueOneofCase))
        };

        string GetFormattedString<TValue>(TValue valueToFormat) where TValue : notnull => _fieldFormatProvider.Format(valueToFormat, format);
    }
}