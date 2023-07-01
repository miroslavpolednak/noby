﻿using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[ApiController]
[Route("api/case")]
public sealed class RealEstateValuationController : ControllerBase
{
    /// <summary>
    /// Založení Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Operace založí nové Ocenění nemovitosti k danému case ID. Jde buď o založení Ocenění nemovitosti k objektu úvěru ze žádosti o úvěr nebo o manuální založení dalšího objektu zajištění.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5A275F67-67AC-4851-8FE9-51F2B685B990"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations")]
    [Consumes("application/json")]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> CreateRealEstateValuation(
        [FromRoute] long caseId, 
        [FromBody] CreateRealEstateValuation.CreateRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(caseId), cancellationToken);

    /// <summary>
    /// Smazání Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Smazání Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=6525504B-D598-4113-AA2A-846571769C40"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeCaseOwner]
    public async Task<IActionResult> DeleteRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRealEstateValuation.DeleteRealEstateValuationRequest(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Získání seznamu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace vrací seznam všech ocenění nemovitostí k danému case ID. Seznam je sloučením ocenění objektů úvěru ze žádosti o úvěr a manuálně zadaných dalších objektů zajištění. Operace nevrací objednávky ocenění, které nevznikly v Noby.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=0DCD023D-ACF8-4744-B198-FE4FC2A84223"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("{caseId:long}/real-estate-valuations")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task GetListRealEstateValuation(
        [FromRoute] long caseId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetListRealEstateValuation.GetListRealEstateValuationRequest(caseId), cancellationToken);

    /// <summary>
    /// Patch developera Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Patch toggle developera na Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=2BD3A207-7DFB-4c5c-B81C-95E99C2D0C58"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPatch("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeCaseOwner]
    public async Task<IActionResult> PatchDeveloperOnRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        [FromBody] PatchDeveloperOnRealEstateValuation.PatchDeveloperOnRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    [HttpPut("real-estate-valuations")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    public async Task UpdateRealEstateValuationDetail([FromBody] UpdateRealEstateValuationDetail.UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(request, cancellationToken);
    }

    private readonly IMediator _mediator;
    public RealEstateValuationController(IMediator mediator) => _mediator = mediator;
}
