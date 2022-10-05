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
    /// Výpočet rozšířené bonity
    /// </summary>
    /// <remarks>
    /// Specs: <a target="_blank" href="https://wiki.kb.cz/confluence/display/HT/CREDIT+WORTHINESS+SERVICE">https://wiki.kb.cz/confluence/display/HT/CREDIT+WORTHINESS+SERVICE</a>
    /// </remarks>
    [HttpPost()]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Contracts.GenerateDocumentIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<Contracts.GenerateDocumentIdResponse> GenerateDocumentId([FromBody] Contracts.GenerateDocumentIdRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
