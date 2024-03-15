using Asp.Versioning;
using NOBY.Api.Endpoints.Refinancing.GenerateRefinancingDocument;
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
    [ProducesResponseType(typeof(GetRefinancingParameters.GetRefinancingParametersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetRefinancingParameters.GetRefinancingParametersResponse> GetRefinancingParameters([FromRoute] long caseId)
        => await _mediator.Send(new GetRefinancingParameters.GetRefinancingParametersRequest(caseId));

    /// <summary>
    /// Seznam možných platností sazby od
    /// </summary>
    /// <remarks>
    /// Vrátí kolekci možných platností sazby od pro konkrétní úvěr.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=7C3EE41F-80E9-4bf2-A0CD-7E6E7F83D704"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/interest-rates-valid-from")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetInterestRatesValidFrom.GetInterestRatesValidFromResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetInterestRatesValidFrom.GetInterestRatesValidFromResponse> GetInterestRatesValidFrom([FromRoute] long caseId)
        => await _mediator.Send(new GetInterestRatesValidFrom.GetInterestRatesValidFromRequest(caseId));

    /// <summary>
    /// Aktuální úroková sazba
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o aktuální úrokové sazbě.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=7C3EE41F-80E9-4bf2-A0CD-7E6E7F83D704"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/interest-rate")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetInterestRate.GetInterestRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetInterestRate.GetInterestRateResponse> GetInterestRate([FromRoute] long caseId)
        => await _mediator.Send(new GetInterestRate.GetInterestRateRequest(caseId));

    /// <summary>
    /// Generování dokumentu Refinancování
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu Retence<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5379DC03-6DFD-411c-9A7C-AB8203677FA9"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("/api/case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/refinancing-documents")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    public async Task GenerateRefinancingDocument(
       long caseId,
       int salesArrangementId,
       [FromBody] GenerateRefinancingDocumentRequest request)
       => await _mediator.Send(request.InfuseCaseId(caseId).InfuseSalesArrangementId(salesArrangementId));
}
