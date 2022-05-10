using Swashbuckle.AspNetCore.Annotations;

namespace DomainServices.RiskIntegrationService.Api.Services;

[ApiController]
[Route("api/testservice")]
public class TestServiceController
    : ControllerBase
{
    /// <summary>
    /// Nazdar endpoint
    /// </summary>
    /// <remarks>Tady mohou byt dalsi poznamky</remarks>
    /// <param name="id">Nejake ID</param>
    /// <param name="request">Telo requestu</param>
    /// <returns>Vraci krasny text</returns>
    [HttpPost("hallo-world")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Test" })]
    [ProducesResponseType(typeof(Contracts.HalloWorldResponse), StatusCodes.Status200OK)]
    public Task<Contracts.HalloWorldResponse> HalloWorld([FromBody] Contracts.HalloWorldRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Contracts.HalloWorldResponse { Name = $"My name is {request.Name}" });
    }
}
