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
        var position = GetPosition(tablePlaceholder, page);
        var size = GetSize(tablePlaceholder);

        _table = new Table2(position.X, position.Y, size.Width, size.Height);

        PrepareTable(_table, tablePlaceholder);

        page.Elements.Add(_table);
    }

    public void DrawOverflowRows(Page page, PdfFormField tablePlaceholder)
    {
        if (_table is null)
            throw new InvalidOperationException("Table is not initialized.");

        var position = GetPosition(tablePlaceholder, page);
        var size = GetSize(tablePlaceholder);

        _table = _table.GetOverflowRows();

        _table.X = position.X;
        _table.Y = position.Y;
        _table.Width = size.Width;
        _table.Height = size.Height;

        page.Elements.Add(_table);
    }

    public void DrawConcludingParagraph(Page page, PdfFormField placeholderField, Func<Page> pageFactory)
    {
        if (_table is null)
            throw new InvalidOperationException("Table is not initialized.");

        if (string.IsNullOrWhiteSpace(_tableData.ConcludingParagraph) || HasOverflowRows)
            return;

        var textArea = CreateTextArea(_table);

        page.Elements.Add(textArea);

        while ((textArea = textArea.GetOverflowTextArea()) != null)
        {
            page = pageFactory();

            var position = GetPosition(placeholderField, page);
            var size = GetSize(placeholderField);

            textArea.X = position.X;
            textArea.Y = position.Y;
            textArea.Width = size.Width;
            textArea.Height = size.Height;

            page.Elements.Add(textArea);
        }
    }

    private static PointF GetPosition(PdfFormField pdfFormField, Page page) => new(pdfFormField.GetX(page), pdfFormField.GetY(page));

    private static SizeF GetSize(PdfFormField pdfFormField) => new(pdfFormField.Width, pdfFormField.Height);

    private void PrepareTable(Table2 table, PdfFormField placeholderField)
    {
        ApplyTableStyle(table, placeholderField);
        CreateColumns(table);
        CreateRows(table);
    }

    private static void ApplyTableStyle(Table2 table, PdfFormField placeholderField)
    {
        table.Border.Width = 0f;
        table.CellDefault.Align = TextAlign.Center;
        table.CellDefault.VAlign = VAlign.Center;
        table.CellDefault.Font = placeholderField.Font;
        table.CellDefault.FontSize = placeholderField.FontSize;
        table.CellDefault.Border.Width = 0.5f;
    }

    private void CreateColumns(Table2 table)
    {
        var row = table.Rows.Add(20, Font.LoadSystemFont(new System.Drawing.Font(table.CellDefault.Font.Name, 9, FontStyle.Bold)), table.CellDefault.FontSize, RgbColor.Black, RgbColor.LightGrey);
        
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

    private TextArea CreateTextArea(Table2 table)
    {
        const float textTopMargin = 5f;

        var visibleHeight = table.GetVisibleHeight();
        var remainingHeight = table.Height - visibleHeight;

        return new TextArea(_tableData.ConcludingParagraph, table.X, table.Y + visibleHeight + textTopMargin, table.Width, remainingHeight - textTopMargin)
        {
            Font = Font.LoadSystemFont(new System.Drawing.Font(table.CellDefault.Font.Name, 9)),
            FontSize = table.CellDefault.FontSize ?? 10,
            Align = TextAlign.Justify
        };
    }
}