using NOBY.Api.Endpoints.DocumentArchive.GetDocument;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
using NOBY.Api.Endpoints.DocumentArchive.SaveDocumentToArchive;
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
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Stream))]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    public async Task<IActionResult> GetDocument(
        [FromRoute] string documentId,
        [FromQuery] FileContentDisposition contentDisposition,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDocumentRequest(documentId), cancellationToken);
        if (contentDisposition == FileContentDisposition.inline)
        {
            return File(result.Content.BinaryData.ToArray(), result.Content.MineType);
        }
        else
        {
            return File(result.Content.BinaryData.ToArray(), result.Content.MineType, result.Metadata.Filename);
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
    public async Task<Guid> UploadDocument(IFormFile file, CancellationToken cancellationToken)
          => await _mediator.Send(new UploadDocumentRequest(file), cancellationToken);

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
    public async Task<IActionResult> SaveDocumentToArchive(
     [FromRoute] long caseId,
     [FromBody] SaveDocumentToArchiveRequest request,
     CancellationToken cancellationToken)
    {
        await _mediator.Send(request?.InfuseCaseId(caseId) ?? throw new CisArgumentException(ErrorCodes.PayloadIsEmpty, "Payload is empty", nameof(request)), 
                             cancellationToken);
        return Accepted();
    }
}
