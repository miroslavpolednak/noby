using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace NOBY.Api.Endpoints.DeedOfOwnership;

[ApiController]
[Route("api/deed-of-ownership")]
public sealed class DeedOfOwnershipController : ControllerBase
{
    /// <summary>
    /// Seznam ISKN id listů vlastnictví pro daný adresní bod
    /// </summary>
    /// <remarks>
    /// Provolání CREM pro získání seznamu ISKN id.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=D5B309D6-5B0C-4fe3-8CCE-50955423F7E4"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("address/{addressPointId:long}/ids")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "List vlastnictví" })]
    [ProducesResponseType(typeof(GetDeedOfOwnershipIds.GetDeedOfOwnershipIdsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetDeedOfOwnershipIds.GetDeedOfOwnershipIdsResponse> GetDeedOfOwnershipIds(
        long addressPointId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetDeedOfOwnershipIds.GetDeedOfOwnershipIdsRequest(addressPointId), cancellationToken);

    /// <summary>
    /// Informace z dokumentu listu vlastnictví
    /// </summary>
    /// <remarks>
    /// Integrace na CREM
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=2B4B2F8E-17B5-48af-A01D-49E60B53E5BA"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramsequence.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("document/content")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "List vlastnictví" })]
    [ProducesResponseType(typeof(GetDeedOfOwnershipDocumentContent.GetDeedOfOwnershipDocumentContentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetDeedOfOwnershipDocumentContent.GetDeedOfOwnershipDocumentContentResponse> GetDeedOfOwnershipDocumentContent(
        int katuzId,
        int deedOfOwnershipNumber,
        long deedOfOwnershipId,
        int nobyDeedOfOwnershipDocumentId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetDeedOfOwnershipDocumentContent.GetDeedOfOwnershipDocumentContentRequest(katuzId, deedOfOwnershipNumber, deedOfOwnershipId, nobyDeedOfOwnershipDocumentId), cancellationToken);

    private readonly IMediator _mediator;
    public DeedOfOwnershipController(IMediator mediator) => _mediator = mediator;
}
