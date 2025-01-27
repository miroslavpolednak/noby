﻿using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.DeedOfOwnership;

[ApiController]
[Route("api/v{v:apiVersion}/deed-of-ownership")]
[ApiVersion(1)]
public sealed class DeedOfOwnershipController(IMediator _mediator) 
    : ControllerBase
{
    /// <summary>
    /// Seznam ISKN id listů vlastnictví pro daný adresní bod
    /// </summary>
    /// <remarks>
    /// Provolání CREM pro získání seznamu ISKN id.
    /// </remarks>
    [HttpGet("address/{addressPointId:long}/ids")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["List vlastnictví"])]
    [ProducesResponseType(typeof(DeedOfOwnershipGetDeedOfOwnershipIdsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=D5B309D6-5B0C-4fe3-8CCE-50955423F7E4")]
    public async Task<DeedOfOwnershipGetDeedOfOwnershipIdsResponse> GetDeedOfOwnershipIds(
        long addressPointId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetDeedOfOwnershipIds.GetDeedOfOwnershipIdsRequest(addressPointId), cancellationToken);

    /// <summary>
    /// Informace z dokumentu listu vlastnictví
    /// </summary>
    /// <remarks>
    /// Integrace na CREM.
    /// </remarks>
    /// <param name="katuzId">KATUZ ID, pětimístné číslo katastrálního území, Id RUIAN katastrálního území</param>
    /// <param name="deedOfOwnershipNumber">Číslo listu vlastnictví (LV)</param>
    /// <param name="deedOfOwnershipId">ISKN ID listu vlastnictví (LV), technický identifikátor katastru</param>
    /// <param name="deedOfOwnershipDocumentId">Noby ID daného záznamu. Určuje jednoznačnou kombinaci cremDeedOfOwnershipDocumentId a RealEstateValuationId (Noby Ocenění) pro případy simulování více možností žádostí s jednou nemovitostí.</param>
    [HttpGet("document/content")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["List vlastnictví"])]
    [ProducesResponseType(typeof(DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=2B4B2F8E-17B5-48af-A01D-49E60B53E5BA")]
    public async Task<DeedOfOwnershipGetDeedOfOwnershipDocumentContentResponse> GetDeedOfOwnershipDocumentContent(
        [FromQuery] int? katuzId,
        [FromQuery] int? deedOfOwnershipNumber,
        [FromQuery] long? deedOfOwnershipId,
        [FromQuery] int? deedOfOwnershipDocumentId,
        [FromQuery] long? cremDeedOfOwnershipDocumentId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetDeedOfOwnershipDocumentContent.GetDeedOfOwnershipDocumentContentRequest(katuzId, deedOfOwnershipNumber, deedOfOwnershipId, deedOfOwnershipDocumentId, cremDeedOfOwnershipDocumentId), cancellationToken);
}
