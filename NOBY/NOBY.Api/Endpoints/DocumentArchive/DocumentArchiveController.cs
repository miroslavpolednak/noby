using NOBY.Api.Endpoints.DocumentArchive.GetDocument;
using NOBY.Api.Endpoints.DocumentArchive.GetDocumentList;
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
    [SwaggerOperation(Tags = new[] { "UC: Dokument" })]
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
    [SwaggerOperation(Tags = new[] { "UC: Dokument" })]
    [ProducesResponseType(typeof(GetDocumentListResponse), StatusCodes.Status200OK)]
    public async Task<GetDocumentListResponse> GetDocumentList([FromRoute] long caseId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDocumentListRequest(caseId), cancellationToken);

}
