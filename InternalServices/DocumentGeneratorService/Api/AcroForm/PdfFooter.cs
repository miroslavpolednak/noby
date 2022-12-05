using System.Globalization;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateTypes;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm;

[ScopedService, SelfService]
public class PdfFooter
{
    private const string FooterIdentifiersFieldName = "ZapatiIdentifikatory";

    private readonly ICodebookServiceClients _codebookService;
    private readonly CultureInfo _cultureInfo;

    private List<DocumentTemplateTypeItem> _templateTypes = null!;

    public PdfFooter(ICodebookServiceClients codebookService)
    {
        _codebookService = codebookService;
        _cultureInfo = (CultureInfo)CultureInfo.GetCultureInfo("cs").Clone();

        _cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
    }

    public async Task FillFooter(Pdf.Document document, DocumentFooter footerData)
    {
        var identifiersField = document.Form.Fields[FooterIdentifiersFieldName];

        _templateTypes = await _codebookService.DocumentTemplateTypes();

        identifiersField.Value = GetIdentifiersText(footerData);
    }

    private string GetIdentifiersText(DocumentFooter footerData)
    {
        var identifiers = new[]
        {
            GetCaseIdIdentifier(footerData.CaseId),
            GetOfferIdIdentifier(footerData.OfferId),
            GetArchiveIdIdentifier(footerData.DocumentId),
            GetDocumentNameIdentifier(footerData.TemplateTypeId, footerData.TemplateVersion),
            GetDocumentDate()
        };

        return string.Join(" | ", identifiers.Where(str => !string.IsNullOrWhiteSpace(str)));
    }

    private string GetDocumentNameIdentifier(int templateTypeId, string templateVersion)
    {
        var templateName = _templateTypes.First(t => t.Id == templateTypeId).ShortName;

        return $"{templateName} {templateVersion}";
    }

    private string? GetCaseIdIdentifier(long? caseId) =>
        caseId is null ? default : $"{PdfTextConstants.CaseIdIdentifierText}:{caseId.Value.ToString(_cultureInfo)}";

    private string? GetOfferIdIdentifier(int? offerId) =>
        offerId is null ? default : $"{PdfTextConstants.OfferIdIdentifierText}:{offerId.Value.ToString(_cultureInfo)}";

    private static string? GetArchiveIdIdentifier(int? archiveId) => archiveId.ToString();

    private string GetDocumentDate() => DateTime.Now.ToString("G", _cultureInfo);
}