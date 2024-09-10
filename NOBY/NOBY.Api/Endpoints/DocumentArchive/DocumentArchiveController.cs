using Asp.Versioning;
using NOBY.Api.Endpoints.DocumentArchive.GetDocument;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.DocumentArchive.SetDocumentStatusInQueue;
using NOBY.Api.Endpoints.DocumentArchive.UploadDocument;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.DocumentArchive;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public class DocumentArchiveController(IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Načtení dokumentu z archivu nebo starbuildu
    /// </summary>
    /// <remarks>
    /// Načtení contentu dokumentu. Vrací se stream binárních dat.<br />
    /// Nenačítají se z eArchivu dokumenty s EaCodeMain.IsVisibleForKb=false. <br /><br />
    /// DocumentId je povinné v kombinaci se source = 0 (eArchiv) a externalId je povinné v kombinaci se source = 1, 2 (Starbuild).<br /><br />
    /// </remarks>
    /// <param name="contentDisposition">0 (Uložit jako ), 1 (Zobrazit v prohlížeči), 0 je default</param>
    /// <param name="source">Zdroj dokumentu (0 - e-archiv; 1 - Starbuild dokumenty (D); 2 - Starbuild přílohy (A), 0 je default</param>
    /// <param name="documentId">Id dokumentu eArchivu</param>
    /// <param name="externalId">Externí ID ePodpisů</param>
    [HttpGet("document")]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=9617EAF8-9876-4444-A130-DFCCD597484D")]

    public async Task<IActionResult> GetDocument(
        [FromQuery] EnumContentDisposition contentDisposition,
        [FromQuery] EnumDocumentSource source,
        [FromQuery] string? documentId,
        [FromQuery] string? externalId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDocumentRequest(source, documentId, externalId), cancellationToken);
        if (contentDisposition == EnumContentDisposition.Inline)
        {
            return File(result.Content.BinaryData, result.Content.MimeType);
        }
        else
        {
            return File(result.Content.BinaryData, result.Content.MimeType, result.Metadata.Filename);
        }
    }

    /// <summary>
    /// Seznam dokumentů ke Case-u
    /// </summary>
    /// <remarks>
    /// Načtení seznamu dokumentů ke Case-u<br />Nevrací se dokumenty s EaCodeMain.IsVisibleForKb=false
    /// </remarks>
    /// <param name="caseId">ID Case-u</param>
    /// <param name="formId">Businessové ID dokumentu, na které chceme zafiltrovat.</param>
    [HttpGet("case/{caseId:long}/documents")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(DocumentArchiveGetDocumentListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=25F73554-B9DB-42a4-8CB5-25FC3B3F6902")]
    public async Task<DocumentArchiveGetDocumentListResponse> GetDocumentList([FromRoute] long caseId, [FromQuery] string? formId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDocumentListRequest(caseId, formId), cancellationToken);

    /// <summary>
    /// Nahrání dokumentu
    /// </summary>
    /// <remarks>
    /// Uložení binárních dat dokumentu do dočasného úložiště.<br /><br />
    /// Zasílá se i jméno souboru viz. filename v multipart/form-data<a href="https://www.rfc-editor.org/rfc/rfc7578"> rfc7578</a><br /><br />
    /// </remarks>
    [HttpPost("document")]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=618F9E36-618D-4dc5-A3C6-38626871504C")]
    public async Task<Guid> UploadDocument(IFormFile file)
          => await _mediator.Send(new UploadDocumentRequest(file));

    /// <summary>
    /// Uložení dokumentů ke Case-u do archivu
    /// </summary>
    /// <remarks>
    /// Uložení dokumentů do archivu
    /// </remarks> 
    [HttpPost("case/{caseId:long}/documents")]
    [SwaggerOperation(Tags = ["Dokument"])]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=5DC440B5-00EB-46dd-8D15-2D7AD41ACD3B")]
    public async Task<IActionResult> SaveDocumentsToArchive(
     [FromRoute] long caseId,
     [FromBody] DocumentArchiveSaveDocumentsToArchiveRequest request)
    {
        await _mediator.Send(request?.InfuseId(caseId) ?? throw new NobyValidationException("Payload is empty"));
        return Accepted();
    }

    /// <summary>
    /// Slouží k nastavení stavu dokumentu ve frontě
    /// </summary>
    /// <remarks>
    /// Nastavení stavu dokumentu ve frontě pro uložení do eArchiv-u
    /// </remarks>
    /// <param name="documentId">ID dokumentu</param>
    /// <param name="statusId">status dokumentu ve frontě</param>
    [HttpPut("document/{documentId}/status/{statusId:int}")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [SwaggerOperation(Tags = ["Dokument"])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=C23F8DBF-9F26-465b-BB34-8736133D020D")]
    public async Task SetDocumentStatusInQueue(
        [FromRoute] string documentId,
        [FromRoute] int statusId,
        CancellationToken cancellationToken)
         => await _mediator.Send(new SetDocumentStatusInQueueRequest(documentId, statusId), cancellationToken);
}
