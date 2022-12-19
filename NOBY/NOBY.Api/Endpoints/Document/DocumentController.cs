using System.Globalization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using CIS.InternalServices.DataAggregator.Configuration;
using DomainServices.CodebookService.Clients;
using CIS.Core.Security;

namespace NOBY.Api.Endpoints.Document;

[ApiController]
[Route("api/document")]
public class DocumentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ICurrentUserAccessor _userAccessor;

    public DocumentController(IMediator mediator, ICodebookServiceClients codebookService, ICurrentUserAccessor userAccessor)
    {
        _mediator = mediator;
        _codebookService = codebookService;
        _userAccessor = userAccessor;
    }

    /// <summary>
    /// Vygenerování dokumentu ze šablony
    /// </summary>
    /// <remarks>
    /// Vygenerování dokumentu ze šablony. A případně uložení do eArchiv-u (např. Nabídka se ukládá).<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=258EEA87-9394-42ec-B51F-C13F091686E0"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="documentTemplateTypeId">ID document template type</param>
    /// <param name="salesArrangementId">ID Sales Arrangement-u</param>
    [HttpGet("template/{documentTemplateTypeId:int}/sales-arrangement/{salesArrangementId:int}")]
    [Produces(MediaTypeNames.Application.Pdf)]
    [SwaggerOperation(Tags = new[] { "Document" })]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> GetDocument(int documentTemplateTypeId, int salesArrangementId, CancellationToken cancellationToken)
    {
        var request = new GetDocument.GetDocumentRequest
        {
            TemplateTypeId = documentTemplateTypeId,
            TemplateVersion = await GetTemplateVersion(documentTemplateTypeId, cancellationToken),
            InputParameters = new InputParameters
            {
                SalesArrangementId = salesArrangementId,
                UserId = _userAccessor.User!.Id
            }
        };

        var response = await _mediator.Send(request, cancellationToken);

        var fileName = await GetFileName(documentTemplateTypeId, response.CaseId, cancellationToken);

        return File(response.Buffer, MediaTypeNames.Application.Pdf, fileName);
    }

    private async Task<string> GetTemplateVersion(int templateTypeId, CancellationToken cancellationToken)
    {
        var versions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        return versions.Where(x => x.DocumentTemplateTypeId == templateTypeId)
                       .Select(x => x.DocumentVersion)
                       .FirstOrDefault() ?? throw new CisValidationException($"Document Version was not found for template type {templateTypeId}");
    }

    private async Task<string> GetFileName(int templateTypeId, long? caseId, CancellationToken cancellationToken)
    {
        var dateTimeText = DateTime.Now.ToString("ddMMyy_HHmmyy", CultureInfo.InvariantCulture);

        return $"{await GetTemplateShortName()}_{caseId}_{dateTimeText}.pdf";

        async Task<string> GetTemplateShortName()
        {
            var templates = await _codebookService.DocumentTemplateTypes(cancellationToken);

            return templates.First(t => t.Id == templateTypeId).ShortName;
        }
    }
}