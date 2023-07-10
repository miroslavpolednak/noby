using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

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
        [Required] [FromBody] CreateRealEstateValuation.CreateRealEstateValuationRequest request, 
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
    /// Získání detailu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace vrací detail Ocenění nemovitostí.
    ///
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=7301EE13-E1C2-4795-A5FA-F8A646C4D057"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(typeof(GetRealEstateValuationDetail.GetRealEstateValuationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeCaseOwner]
    public async Task<GetRealEstateValuationDetail.GetRealEstateValuationDetailResponse> GetRealEstateValuationDetail(
        long caseId, 
        int realEstateValuationId, 
        CancellationToken cancellationToken) 
        => await _mediator.Send(new GetRealEstateValuationDetail.GetRealEstateValuationDetailRequest(caseId, realEstateValuationId), cancellationToken);

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
    [ProducesResponseType(typeof(List<Dto.RealEstateValuation.RealEstateValuationListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<Dto.RealEstateValuation.RealEstateValuationListItem>> GetListRealEstateValuation(
        [FromRoute] long caseId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRealEstateValuationList.GetRealEstateValuationListRequest(caseId), cancellationToken);

    /// <summary>
    /// Aktualizace detailu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace edituje detail Ocenění nemovitostí.
    ///
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=39883A18-AA29-4f7d-9E4E-BC2D5F81B115"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeCaseOwner]
    public async Task<IActionResult> UpdateRealEstateValuationDetail(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [Required] [FromBody] UpdateRealEstateValuationDetail.UpdateRealEstateValuationDetailRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Patch developera Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Patch toggle developera na Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=2BD3A207-7DFB-4c5c-B81C-95E99C2D0C58"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPatch("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/developer")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeCaseOwner]
    public async Task<IActionResult> PatchDeveloperOnRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        [Required] [FromBody] PatchDeveloperOnRealEstateValuation.PatchDeveloperOnRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    private readonly IMediator _mediator;
    public RealEstateValuationController(IMediator mediator) => _mediator = mediator;
}
