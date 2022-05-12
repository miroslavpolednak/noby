using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Endpoints;

[Authorize]
[ApiController]
[Route("api/testservice")]
public class TestServiceController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public TestServiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Nazdar endpoint
    /// </summary>
    /// <remarks>Tady mohou byt dalsi poznamky</remarks>
    /// <param name="request">Telo requestu</param>
    /// <returns>Vraci krasny text</returns>
    [HttpPost("hallo-world")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Test" })]
    [ProducesResponseType(typeof(Contracts.HalloWorldResponse), StatusCodes.Status200OK)]
    public async Task<Contracts.HalloWorldResponse> HalloWorld([FromBody] Contracts.HalloWorldRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
