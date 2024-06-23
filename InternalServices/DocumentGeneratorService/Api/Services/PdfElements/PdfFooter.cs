using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.PageElements.BarCoding;
using CIS.InternalServices.DocumentGeneratorService.Api.Extensions;
using CIS.InternalServices.DocumentGeneratorService.Api.Model;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

namespace CIS.InternalServices.DocumentGeneratorService.Api.Services.PdfElements;

public class PdfFooter
{
    private const string FooterIdentifiersFieldName = "ZapatiIdentifikatory";
    private const string FooterBarcodeFieldName = "ZapatiBarcode";
    private const string FooterPageNumberFieldName = "ZapatiCisloStranky";
    private const string PageNumberFormat = "%%CP%%/%%TP%%";

    private readonly CultureInfo _cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("cs").Clone();

    public void FillFooter(PdfTemplate template, MergeDocument mergeDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, GenerateDocumentRequest request)
    {
        mergeDocument.Template = new Template();

        FillFooterIdentifiers(template, mergeDocument, fieldMap, request);
        DrawBarcode(mergeDocument, fieldMap, request.DocumentFooter);
        FillFooterPageNumber(mergeDocument, fieldMap);
    }

    private void FillFooterIdentifiers(PdfTemplate template, MergeDocument mergeDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, GenerateDocumentRequest request)
    {
        var identifiersField = fieldMap[FooterIdentifiersFieldName].Field;
        var identifiersText = GetIdentifiersText(template, request);

        var textField = new TextArea(identifiersText,
                                     identifiersField.Coordinates.X,
                                     identifiersField.Coordinates.Y,
                                     identifiersField.Coordinates.Width,
                                     identifiersField.Coordinates.Height,
                                     FontHelper.ParseOpenTypeFont(identifiersField.FontName),
                                     identifiersField.FontSize,
                                     Pdf.TextAlign.Right);

        mergeDocument.Template.Elements.Add(textField);
    }

    private static void DrawBarcode(MergeDocument mergeDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap, DocumentFooter footer)
    {
        if (string.IsNullOrWhiteSpace(footer.BarcodeText))
            return;

        var barcodeField = fieldMap.GetValueOrDefault(FooterBarcodeFieldName)?.Field;

        if (barcodeField is null)
            return;

        var barcode = new Code128(footer.BarcodeText, barcodeField.Coordinates.X, barcodeField.Coordinates.Y, barcodeField.Coordinates.Height)
        {
            TextAlign = Align.Right,
            FontSize = 8,
            Font = GeneratorVariables.Arial.GetFont()
        };

        barcode.X += barcodeField.Coordinates.Width - 2 - barcode.GetSymbolWidth();

        mergeDocument.Template.Elements.Add(barcode);
    }

    private static void FillFooterPageNumber(MergeDocument mergeDocument, IReadOnlyDictionary<string, DocumentMapItem> fieldMap)
    {
        var pageNumberField = fieldMap.GetValueOrDefault(FooterPageNumberFieldName)?.Field;

        if (pageNumberField is null)
            return;

        var pageNumberingLabel = new PageNumberingLabel(PageNumberFormat,
                                                        pageNumberField.Coordinates.X,
                                                        pageNumberField.Coordinates.Y,
                                                        pageNumberField.Coordinates.Width - 2,
                                                        pageNumberField.Coordinates.Height,
                                                        FontHelper.ParseOpenTypeFont(pageNumberField.FontName),
                                                        pageNumberField.FontSize,
                                                        Pdf.TextAlign.Right);

        mergeDocument.Template.Elements.Add(pageNumberingLabel);
    }

    private string GetIdentifiersText(PdfTemplate template, GenerateDocumentRequest request)
    {
        var footer = request.DocumentFooter;

        var identifiers = new[]
        {
            GetCaseIdIdentifier(footer.CaseId),
            GetOfferIdIdentifier(footer.OfferId, footer.SalesArrangementId),
            GetSalesArrangementIdIdentifier(footer.SalesArrangementId),
            GetDocumentOnSaIdIdentifier(footer.DocumentOnSaId),
            GetDocumentNameIdentifier(template),
            GetDocumentDate()
        };

        return string.Join(" | ", identifiers.Where(str => !string.IsNullOrWhiteSpace(str)));
    }

    private static string GetDocumentNameIdentifier(PdfTemplate template) => $"{template.Name} {template.Version}{template.Variant}";

    private string? GetCaseIdIdentifier(long? caseId) =>
        caseId is null ? default : $"{PdfTextConstants.CaseIdIdentifierText}:{caseId.Value.ToString(_cultureInfo)}";
    private string? GetOfferIdIdentifier(int? offerId, int? salesArrangementId) =>
        offerId is null || salesArrangementId.HasValue ? default : $"{PdfTextConstants.OfferIdIdentifierText}:{offerId.Value.ToString(_cultureInfo)}";
    private string? GetSalesArrangementIdIdentifier(int? salesArrangementId) =>
        salesArrangementId is null ? default : $"{PdfTextConstants.SalesArrangementIdIdentifierText}:{salesArrangementId.Value.ToString(_cultureInfo)}";
    private string? GetDocumentOnSaIdIdentifier(int? documentOnSaId) =>
        documentOnSaId is null ? default : $"{PdfTextConstants.DocumentOnSaIdIdentifierText}:{documentOnSaId.Value.ToString(_cultureInfo)}";

    private string GetDocumentDate() => DateTime.Now.ToString("G", _cultureInfo);
}