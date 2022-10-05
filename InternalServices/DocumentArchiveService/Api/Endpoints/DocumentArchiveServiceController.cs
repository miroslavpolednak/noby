using Microsoft.AspNetCore.Mvc;

namespace CIS.InternalServices.DocumentArchiveService.Api.Endpoints;

[Authorize]
[ApiController]
[Route("v1")]
public class DocumentArchiveServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentArchiveServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Generování DocumentId eArchivu (prodloužená ruka eArchivu)
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/display/HT/generateDocumentId">https://wiki.kb.cz/display/HT/generateDocumentId</a>
    /// </remarks>
    [HttpPost()]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Contracts.GenerateDocumentIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<Contracts.GenerateDocumentIdResponse> GenerateDocumentId([FromBody] Contracts.GenerateDocumentIdRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(new GenerateDocumentId.GenerateDocumentIdMediatrRequest(request), cancellationToken);
}
