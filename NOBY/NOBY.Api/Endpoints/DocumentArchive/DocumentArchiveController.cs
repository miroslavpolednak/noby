﻿using CIS.Infrastructure.gRPC;
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
    /// Načtení dokumentu z e-archivu nebo ze systému ePodpisů
    /// </summary>
    /// <remarks>
    ///Načtení contentu dokumentu.Vrací se stream binárních dat.<br />
    ///Pro dokumenty z e-archivu se nenačítají dokumenty s EaCodeMain.IsVisibleForKb=false.<br /><br />
    /// <a href ="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=9617EAF8-9876-4444-A130-DFCCD597484D"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="documentId">ID dokumentu</param>
    /// <param name="contentDisposition">0 (Uložit jako ), 1 (Zobrazit v prohlížeči), 0 je default</param>
    /// <param name="source">Zdroj dokumentu (0 - e-archiv; 1 - fronta do systému ePodpisů; 2 - systém ePodpisů; 0 je default</param>
    [HttpGet("document/{documentId}")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocument(
        [FromRoute] string documentId,
        [FromQuery] FileContentDisposition contentDisposition,
        [FromQuery] DocumentSource source, // added prep. according to HFICH-5628 (without implementation)
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
    /// Načtení seznamu dokumentů ke Case-u<br />Nevrací se dokumenty s EaCodeMain.IsVisibleForKb=false<br /><br />
    /// <a href ="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=25F73554-B9DB-42a4-8CB5-25FC3B3F6902"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
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
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5DC440B5-00EB-46dd-8D15-2D7AD41ACD3B"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks> 
    [HttpPost("case/{caseId:long}/documents")]
    [SwaggerOperation(Tags = new[] { "Dokument" })]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> SaveDocumentsToArchive(
     [FromRoute] long caseId,
     [FromBody] SaveDocumentsToArchiveRequest request)
    {
        await _mediator.Send(request?.InfuseId(caseId) ?? throw new NobyValidationException("Payload is empty"));
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
