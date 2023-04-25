using CIS.Infrastructure.gRPC;
using NOBY.Api.Endpoints.DocumentArchive.GetDocument;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.DocumentArchive.SaveDocumentsToArchive;
using NOBY.Api.Endpoints.DocumentArchive.SetDocumentStatusInQueue;
using NOBY.Api.Endpoints.DocumentArchive.UploadDocument;
using NOBY.Api.Endpoints.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.DocumentArchive;

[ApiController]
[Route("api")]
public class DocumentArchiveController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentArchiveController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Načtení dokumentu z archivu
    /// </summary>
    /// <remarks>
    /// Načtení contentu dokumentu. Vrací se steam binárních dat.<br/>
    /// Nenačítají se dokumenty s EaCodeMain.IsVisibleForKb=false <br/>
    /// <i>DS:</i> DocumentArchiveService/getDocument
    /// </remarks>
    /// <param name="documentId">ID dokumentu</param>
    /// <param name="contentDisposition">0 (Uložit jako ), 1 (Zobrazit v prohlížeči), 0 je default</param>
    [HttpGet("document/{documentId}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocument(
        [FromRoute] string documentId,
        [FromQuery] FileContentDisposition contentDisposition,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDocumentRequest(documentId), cancellationToken);
        if (contentDisposition == FileContentDisposition.inline)
        {
            return File(result.Content.BinaryData.ToArrayUnsafe(), result.Content.MineType);
        }
        else
        {
            return File(result.Content.BinaryData.ToArrayUnsafe(), result.Content.MineType, result.Metadata.Filename);
        }
    }

    /// <summary>
    /// Seznam dokumentů ke Case-u
    /// </summary>
    /// <remarks>
    /// Načtení seznamu dokumentů ke Case-u z archivu<br/>
    /// Nevrací se dokumenty s EaCodeMain.IsVisibleForKb=false <br/>
    /// <i>DS:</i>DocumentArchiveService/getDocumentList
    /// </remarks> 
    /// <param name="caseId">ID Case-u</param>
    [HttpGet("case/{caseId:long}/documents")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(GetDocumentListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetDocumentListResponse> GetDocumentList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDocumentListRequest(caseId), cancellationToken);

    /// <summary>
    /// Nahrání dokumentu
    /// </summary>
    /// <remarks>
    /// Uložení binárních dat dokumentu do dočasného úložiště.
    /// </remarks> 
    [HttpPost("document")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<Guid> UploadDocument(IFormFile file)
          => await _mediator.Send(new UploadDocumentRequest(file));

    /// <summary>
    /// Uložení dokumentů ke Case-u do archivu
    /// </summary>
    /// <remarks>
    /// Uložení dokumentů do archivu <br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=5DC440B5-00EB-46dd-8D15-2D7AD41ACD3B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks> 
    [HttpPost("case/{caseId:long}/documents")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> SaveDocumentsToArchive(
     [FromRoute] long caseId,
     [FromBody] SaveDocumentsToArchiveRequest request)
    {
        await _mediator.Send(request?.InfuseCaseId(caseId) ?? throw new NobyValidationException("Payload is empty"));
        return Accepted();
    }

    /// <summary>
    /// Slouží k nastavení stavu dokumentu ve frontě
    /// </summary>
    /// <remarks>
    /// Nastavení stavu dokumentu ve frontě pro uložení do eArchiv-u
    /// <br /><br /><a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C23F8DBF-9F26-465b-BB34-8736133D020D"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("document/{documentId}/status/{statusId:int}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task SetDocumentStatusInQueue(
        [FromRoute] string documentId,
        [FromRoute] int statusId,
        CancellationToken cancellationToken)
         => await _mediator.Send(new SetDocumentStatusInQueueRequest(documentId, statusId), cancellationToken);
}
