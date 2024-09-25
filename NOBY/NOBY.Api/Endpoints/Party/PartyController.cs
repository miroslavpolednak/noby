using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Party;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}")]
public class PartyController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Našeptávač jmen a IČ
    /// </summary>
    /// <remarks>
    /// Provoláním získáme 0-N dvojic jméno-IČ
    /// </remarks>
    [HttpPost("party/search")]
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Klient - příjem"])]
    [ProducesResponseType(typeof(List<PartySearchPartiesResponse>), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=815A085A-B652-489e-987D-CBB0517846C6")]
    public async Task<List<PartySearchPartiesResponse>> SearchParties(
        [FromBody] PartySearchPartiesRequest request,
        CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}
