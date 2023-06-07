using System.Globalization;
using ceTe.DynamicPDF.Merger.Forms;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.PageElements.BarCoding;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[ScopedService, SelfService]
public class PdfFooter
{
    private const string FooterIdentifiersFieldName = "ZapatiIdentifikatory";
    private const string FooterBarcodeFieldName = "ZapatiBarcode";
    private const string FooterPageNumberFieldName = "ZapatiCisloStranky";
    private const string PageNumberFormat = "%%CP%%/%%TP%%";

    private readonly ICodebookServiceClient _codebookService;
    private readonly CultureInfo _cultureInfo;

    private List<DocumentTypesResponse.Types.DocumentTypeItem> _templateTypes = null!;
    private List<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem> _templateVersions = null!;
    private List<DocumentTemplateVariantsResponse.Types.DocumentTemplateVariantItem> _templateVariants = null!;

    public PdfFooter(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
        _cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("cs").Clone();

        _cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
    }

    public async Task FillFooter(FinalDocument finalDocument, GenerateDocumentRequest request)
    {
        _templateTypes = await _codebookService.DocumentTypes();
        _templateVersions = await _codebookService.DocumentTemplateVersions();
        _templateVariants = await _codebookService.DocumentTemplateVariants();

        finalDocument.Document.Template = new Template();

        FillFooterIdentifiers(finalDocument.Document.Form.Fields, request);
        DrawBarcode(finalDocument, request.DocumentFooter);
        FillFooterPageNumber(finalDocument);
    }

    private void FillFooterIdentifiers(FormFieldList fields, GenerateDocumentRequest request)
    {
        var identifierFields = GetAllFields(fields, FooterIdentifiersFieldName);

        var identifiersText = GetIdentifiersText(request);

        foreach (var field in identifierFields)
            field.Value = identifiersText;
    }

    private static void DrawBarcode(FinalDocument finalDocument, DocumentFooter footer)
    {
        if (string.IsNullOrWhiteSpace(footer.BarcodeText))
            return;

        var barcodeFields = GetAllFields(finalDocument.Document.Form.Fields, FooterBarcodeFieldName).ToList();

        var originalBarcodeField = finalDocument.PdfDocumentParts
                                                .SelectMany(original => GetOriginalFields(barcodeFields, original))
                                                .FirstOrDefault(original => original is not null);

        if (originalBarcodeField is null)
            return;

        var page = finalDocument.Document.Pages[0];
        var barcodeField = originalBarcodeField.ChildFields is null ? originalBarcodeField : originalBarcodeField.ChildFields[0];

        var barcode = new Code128(footer.BarcodeText, barcodeField.GetX(page), barcodeField.GetY(page), barcodeField.Height)
        {
            TextAlign = Align.Right,
            FontSize = 8
        };

        barcode.X += barcodeField.Width - 2 - barcode.GetSymbolWidth();

        finalDocument.Document.Template.Elements.Add(barcode);

        barcodeFields.ForEach(f => f.Output = FormFieldOutput.Remove);
    }

    private static void FillFooterPageNumber(FinalDocument finalDocument)
    {
        var pageNumberFields = GetAllFields(finalDocument.Document.Form.Fields, FooterPageNumberFieldName).ToList();

        var originalField = finalDocument.PdfDocumentParts
                                         .SelectMany(original => GetOriginalFields(pageNumberFields, original))
                                         .FirstOrDefault(original => original is not null);


        if (originalField is null)
            return;

        var page = finalDocument.Document.Pages[0];

        var field = originalField.ChildFields is null ? originalField : originalField.ChildFields[0];

        var pageNumberingLabel = new PageNumberingLabel(PageNumberFormat,
                                                        field.GetX(page),
                                                        field.GetY(page),
                                                        field.Width - 2,
                                                        field.Height,
                                                        field.Font,
                                                        field.FontSize,
                                                        Pdf.TextAlign.Right);

        finalDocument.Document.Template.Elements.Add(pageNumberingLabel);

        pageNumberFields.ForEach(field => field.Output = FormFieldOutput.Remove);
    }

    private static IEnumerable<FormField> GetAllFields(FormFieldList fields, string fieldName) =>
        fields.Cast<FormField>()
              .Concat(fields.Cast<FormField>().SelectMany(f => f.ChildFields.Cast<FormField>()))
              .Where(f => f.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

    private static IEnumerable<PdfFormField?> GetOriginalFields(IEnumerable<FormField> fields, PdfDocument originalDocument)
    {
        return fields.Select(SelectOriginalField);

        PdfFormField? SelectOriginalField(FormField field) => field.Parent?.HasChildFields ?? false
            ? originalDocument.Form.Fields[field.Parent.Name].ChildFields[field.Name]
            : originalDocument.Form.Fields[field.Name];
    }

    private string GetIdentifiersText(GenerateDocumentRequest request)
    {
        var footer = request.DocumentFooter;

        var identifiers = new[]
        {
            GetCaseIdIdentifier(footer.CaseId),
            GetOfferIdIdentifier(footer.OfferId, footer.SalesArrangementId),
            GetSalesArrangementIdIdentifier(footer.SalesArrangementId),
            GetDocumentNameIdentifier(request.DocumentTypeId, request.DocumentTemplateVersionId, request.DocumentTemplateVariantId),
            GetDocumentDate()
        };

        return string.Join(" | ", identifiers.Where(str => !string.IsNullOrWhiteSpace(str)));
    }

    private string GetDocumentNameIdentifier(int documentTypeId, int documentTemplateVersionId, int? documentTemplateVariantId)
    {
        var templateName = _templateTypes.First(t => t.Id == documentTypeId).ShortName;
        var versionName = _templateVersions.First(t => t.Id == documentTemplateVersionId).DocumentVersion;

        var variantName = string.Empty;
        if (documentTemplateVariantId.HasValue)
            variantName = _templateVariants.First(t => t.Id == documentTemplateVariantId).DocumentVariant;

        return $"{templateName} {versionName}{variantName}";
    }

    private string? GetCaseIdIdentifier(long? caseId) =>
        caseId is null ? default : $"{PdfTextConstants.CaseIdIdentifierText}:{caseId.Value.ToString(_cultureInfo)}";

    private string? GetOfferIdIdentifier(int? offerId, int? salesArrangementId) =>
        offerId is null || salesArrangementId.HasValue ? default : $"{PdfTextConstants.OfferIdIdentifierText}:{offerId.Value.ToString(_cultureInfo)}";

    private string? GetSalesArrangementIdIdentifier(int? salesArrangementId) =>
        salesArrangementId is null ? default : $"{PdfTextConstants.SalesArrangementIdIdentifierText}:{salesArrangementId.Value.ToString(_cultureInfo)}";

    private string GetDocumentDate() => DateTime.Now.ToString("G", _cultureInfo);
}