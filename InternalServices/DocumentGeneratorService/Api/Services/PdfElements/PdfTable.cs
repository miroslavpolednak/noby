using ceTe.DynamicPDF.Merger.Forms;
using ceTe.DynamicPDF.PageElements;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;
using System.ComponentModel;
using CIS.InternalServices.DocumentGeneratorService.Api.Extensions;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfElements;

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

    public void DrawTable(Page page, FieldInfo tableFieldInfo)
    {
        _table = new Table2(tableFieldInfo.Coordinates.X, tableFieldInfo.Coordinates.Y, tableFieldInfo.Coordinates.Width, tableFieldInfo.Coordinates.Height);

        PrepareTable(_table, tableFieldInfo);

        page.Elements.Add(_table);
    }

    public void DrawOverflowRows(Page page, FieldInfo tableFieldInfo)
    {
        if (_table is null)
            throw new InvalidOperationException("Table is not initialized.");

        _table = _table.GetOverflowRows();

        _table.X = tableFieldInfo.Coordinates.X;
        _table.Y = tableFieldInfo.Coordinates.Y;
        _table.Width = tableFieldInfo.Coordinates.Width;
        _table.Height = tableFieldInfo.Coordinates.Height;

        page.Elements.Add(_table);
    }

    public void DrawConcludingParagraph(Page page, FieldInfo tableFieldInfo, Func<Page> pageFactory)
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

            textArea.X = tableFieldInfo.Coordinates.X;
            textArea.Y = tableFieldInfo.Coordinates.Y;
            textArea.Width = tableFieldInfo.Coordinates.Width;
            textArea.Height = tableFieldInfo.Coordinates.Height;

            page.Elements.Add(textArea);
        }
    }

    private void PrepareTable(Table2 table, FieldInfo tableFieldInfo)
    {
        ApplyTableStyle(table, tableFieldInfo);
        CreateColumns(table);
        CreateRows(table);
    }

    private static void ApplyTableStyle(Table2 table, FieldInfo tableFieldInfo)
    {
        table.Border.Width = 0f;
        table.CellDefault.Align = Pdf.TextAlign.Center;
        table.CellDefault.VAlign = Pdf.VAlign.Center;
        table.CellDefault.Font = GeneratorVariables.Arial.GetFont();
        table.CellDefault.Font = FontHelper.ParseOpenTypeFont(tableFieldInfo.FontName);
        table.CellDefault.FontSize = tableFieldInfo.FontSize;
        table.CellDefault.Border.Width = 0.5f;
    }

    private void CreateColumns(Table2 table)
    {
        var row = table.Rows.Add(20, GeneratorVariables.ArialBold.GetFont(), table.CellDefault.FontSize, RgbColor.Black, RgbColor.LightGrey);

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
            GenericTableRowValue.ValueOneofCase.Text => format is null ? value.Text : string.Format(CultureInfo.InvariantCulture, format, value.Text),
            GenericTableRowValue.ValueOneofCase.Date => GetFormattedString<DateTime>(value.Date),
            GenericTableRowValue.ValueOneofCase.Number => GetFormattedString(value.Number),
            GenericTableRowValue.ValueOneofCase.DecimalNumber => GetFormattedString<decimal>(value.DecimalNumber),
            _ => throw new InvalidEnumArgumentException(nameof(value.ValueCase), (int)value.ValueCase, typeof(GenericTableRowValue.ValueOneofCase))
        };

        string GetFormattedString<TValue>(TValue valueToFormat) where TValue : notnull => _fieldFormatProvider.Format(valueToFormat, format);
    }

    private TextArea CreateTextArea(Table2 table)
    {
        const float TextTopMargin = 5f;

        var visibleHeight = table.GetVisibleHeight();
        var remainingHeight = table.Height - visibleHeight;

        return new TextArea(_tableData.ConcludingParagraph, table.X, table.Y + visibleHeight + TextTopMargin, table.Width, remainingHeight - TextTopMargin)
        {
            Font = GeneratorVariables.Arial.GetFont(),
            FontSize = table.CellDefault.FontSize ?? 10,
            Align = Pdf.TextAlign.Justify
        };
    }
}