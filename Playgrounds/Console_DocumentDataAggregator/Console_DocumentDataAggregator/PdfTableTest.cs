using ceTe.DynamicPDF;
using ceTe.DynamicPDF.Forms;
using ceTe.DynamicPDF.Merger;
using ceTe.DynamicPDF.PageElements;

namespace Console_DocumentDataAggregator;

public class PdfTableTest
{
    public void Test()
    {
        var pdfDocument = new PdfDocument("D:\\MPSS\\TestPdf\\SPLKALHU\\SPLKALHU_S1.pdf");

        var document = new MergeDocument(pdfDocument);

        var tableElement = pdfDocument.Form.Fields["SplatkovyKalendar"];

        var x = tableElement.GetX(document.Pages[0]);
        var y = tableElement.GetY(document.Pages[0]);

        var table = new Table2(x, y, tableElement.Width, tableElement.Height);

        // Add columns to the table
        table.Columns.Add(table.Width / 3);
        table.Columns.Add(table.Width / 3);
        table.Columns.Add(table.Width / 3);
        table.Border.Width = 0.5f;
        table.CellDefault.Align = TextAlign.Center;
        table.CellDefault.VAlign = VAlign.Center;
        table.CellDefault.Font = Font.Helvetica;
        table.CellDefault.Border.Width = 0.5f;

        // Add rows to the table and add cells to the rows
        Row2 row1 = table.Rows.Add(20, Font.HelveticaBold, table.CellDefault.FontSize, RgbColor.Black, RgbColor.LightGrey);
        row1.Cells.Add("Header 1");
        row1.Cells.Add("Header 2");
        row1.Cells.Add("Header 3");

        Row2 row2 = table.Rows.Add(20);
        row2.Cells.Add("Item 1");
        row2.Cells.Add("Item 2");
        row2.Cells.Add("Item 3");


        document.Pages[0].Elements.Add(table);

        document.Form.Fields["SplatkovyKalendar"].Output = FormFieldOutput.Remove;

        document.Draw("D:\\MPSS\\TestPdf\\SPLKALHU\\Test.pdf");
    }
}