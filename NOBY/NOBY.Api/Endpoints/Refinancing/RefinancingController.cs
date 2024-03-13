using Asp.Versioning;
using NOBY.Api.Endpoints.Refinancing.GetProcessDetail;
using NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;
using Swashbuckle.AspNetCore.Annotations;

namespace NOBY.Api.Endpoints.Refinancing;

[ApiController]
[Route("api")]
[ApiVersion(1)]
public sealed class RefinancingController : ControllerBase
{
    private readonly IMediator _mediator;
    public RefinancingController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Žádosti o změnu sazby
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací a žádostí ke změnám sazeb.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=EBF744FA-9F0F-421e-89F8-CFFFAEC76BB1"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/refinancing-parameters")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetRefinancingParametersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetRefinancingParametersResponse> GetRefinancingParameters(
        [FromRoute] long caseId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRefinancingParametersRequest(caseId), cancellationToken);

    /// <summary>
    /// Detail workflow procesu dotažený ze SB.
    /// </summary>
    /// <remarks>
    /// Detail workflow procesu dotažený ze SB. <br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=DB093BE8-72C4-4bd2-9FD0-8E7393C2F8B5"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a><br/><br/>
    /// <strong style="color:red;">Required permissions</strong><br/>CaseOwnerCheck()
    /// </remarks>
    [HttpGet("case/{caseId:long}/processes/{processId:long}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetProcessDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetProcessDetailResponse> GetProcessDetail(
        [FromRoute] long caseId,
        [FromRoute] long processId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetProcessDetailRequest(caseId, processId), cancellationToken);
}
