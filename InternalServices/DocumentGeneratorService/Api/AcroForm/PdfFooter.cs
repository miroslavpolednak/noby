using System.Globalization;
using ceTe.DynamicPDF.Merger.Forms;
using ceTe.DynamicPDF.PageElements;
using CIS.InternalServices.DocumentGeneratorService.Api.Storage;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[ScopedService, SelfService]
public class PdfFooter
{
    private const string FooterIdentifiersFieldName = "ZapatiIdentifikatory";
    private const string FooterPageNumberFieldName = "ZapatiCisloStranky";
    private const string PageNumberFormat = "%%CP%%/%%TP%%";

    private readonly ICodebookServiceClients _codebookService;
    private readonly CultureInfo _cultureInfo;

    private List<DocumentTypeItem> _templateTypes = null!;
    private List<DocumentTemplateVersionItem> _templateVersions = null!;
    private List<DocumentTemplateVariantItem> _templateVariants = null!;

    public PdfFooter(ICodebookServiceClients codebookService)
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

        FillFooterIdentifiers(finalDocument.Document.Form.Fields, request);
        FillFooterPageNumber(finalDocument);
    }

    private void FillFooterIdentifiers(FormFieldList fields, GenerateDocumentRequest request)
    {
        var identifierFields = GetAllFields(fields, FooterIdentifiersFieldName);

        var identifiersText = GetIdentifiersText(request);

        foreach (var field in identifierFields)
            field.Value = identifiersText;
    }

    private void FillFooterPageNumber(FinalDocument finalDocument)
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

        finalDocument.Document.Template = new Template { Elements = { pageNumberingLabel } }; ;

        pageNumberFields.ForEach(field => field.Output = FormFieldOutput.Remove);
    }

    private static IEnumerable<FormField> GetAllFields(FormFieldList fields, string fieldName) =>
        fields.Cast<FormField>()
              .Concat(fields.Cast<FormField>().SelectMany(f => f.ChildFields.Cast<FormField>()))
              .Where(f => f.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

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
            GetOfferIdIdentifier(footer.OfferId),
            footer.DocumentId,
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

    private string? GetOfferIdIdentifier(int? offerId) =>
        offerId is null ? default : $"{PdfTextConstants.OfferIdIdentifierText}:{offerId.Value.ToString(_cultureInfo)}";

    private string GetDocumentDate() => DateTime.Now.ToString("G", _cultureInfo);
}