using Asp.Versioning;
using NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;
using Swashbuckle.AspNetCore.Annotations;
using NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;
using NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;
using NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;
using NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;
using NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;

namespace NOBY.Api.Endpoints.Refinancing;

[ApiController]
[Route("api")]
[ApiVersion(1)]
public sealed class RefinancingController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Seznam dostupných délek fixací pro refixace
    /// </summary>
    /// <remarks>
    /// Délky fixace jsou uvedeny v měsících.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A4BCB0C7-506D-4edb-ADCD-66BA06A5D1DD"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-refixation/available-fixed-rate-periods")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetAvailableFixedRatePeriodsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetAvailableFixedRatePeriodsResponse> GetAvailableFixedRatePeriods([FromRoute] long caseId)
        => await _mediator.Send(new GetAvailableFixedRatePeriodsRequest(caseId));

    /// <summary>
    /// Vytvoří odpovědní kód
    /// </summary>
    /// <remarks>
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=BA03AE2F-C91E-4308-9C92-C24A7CF66D08"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("case/{caseId:long}/mortgage/send-response-code")]
    [Consumes("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMortgageResponseCode([FromRoute] long caseId, [FromBody] SendMortgageResponseCode.SendMortgageResponseCodeRequest? request)
    {
        await _mediator.Send((request ?? new SendMortgageResponseCode.SendMortgageResponseCodeRequest()).InfuseId(caseId));
        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=32807AA6-9D09-4c6e-9BBB-F21A962437FE"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("case/{caseId:long}/sales-arrangement/{salesArrangementId:long}/update-mortgage-refixation")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(UpdateMortgageRefixationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UpdateMortgageRefixationResponse> UpdateMortgageRefixation([FromRoute] long caseId, [FromRoute] int salesArrangementId, [FromBody] UpdateMortgageRefixationRequest? request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId, salesArrangementId));

    /// <summary>
    /// Detail mimořádné splátky
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném procesu mimořádné splátky.
    /// 
    /// <a href=""><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-extra-payment/{processId:long}")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageExtraPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetMortgageExtraPaymentResponse> GetMortgageExtraPayment([FromRoute] long caseId, [FromQuery] long processId)
        => await _mediator.Send(new GetMortgageExtraPaymentRequest(caseId, processId));

    /// <summary>
    /// Detail refixace
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném refixačním procesu.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=06834209-6BE1-42bf-8C6A-DD4D4371B14F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-refixation")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageRefixationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetMortgageRefixationResponse> GetMortgageRefixation([FromRoute] long caseId, [FromQuery] long? processId = null)
        => await _mediator.Send(new GetMortgageRefixationRequest(caseId, processId));

    /// <summary>
    /// Detail retence
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném retenčním procesu.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=E0F5C17D-A6F5-4713-93DC-73E06D98AD09"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-retention/{processId:long}")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageRetentionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetMortgageRetentionResponse> GetMortgageRetention([FromRoute] long caseId, [FromRoute] long processId)
        => await _mediator.Send(new GetMortgageRetentionRequest(caseId, processId));

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
    public async Task<GetRefinancingParametersResponse> GetRefinancingParameters([FromRoute] long caseId)
        => await _mediator.Send(new GetRefinancingParametersRequest(caseId));

    /// <summary>
    /// Seznam možných platností sazby od
    /// </summary>
    /// <remarks>
    /// Vrátí kolekci možných platností sazby od pro konkrétní úvěr.
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
    /// Sdělení nabídek refixací.
    /// </summary>
    /// <remarks>
    /// Smaže sdělené nabídky (pokud nejsou shodné s aktuálními) a označí aktuální paletu nabídek jako sdělenou.<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=95CE2766-FF1F-4267-9873-177189302EDD"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("case/{caseId:long}/mortgage-refixation-offer-list/communicate")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(CommunicateMortgageRefixation.CommunicateMortgageRefixationResponse), StatusCodes.Status200OK)]
    public async Task<CommunicateMortgageRefixation.CommunicateMortgageRefixationResponse> CommunicateMortgageRefixation(long caseId) => 
        await _mediator.Send(new CommunicateMortgageRefixation.CommunicateMortgageRefixationRequest { CaseId = caseId});

    /// <summary>
    /// Generování dokumentu Retenčního dodatku
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu Retenčního dodatku<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5379DC03-6DFD-411c-9A7C-AB8203677FA9"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("/api/case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/retention-document")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    public async Task GenerateRetentionDocument(long caseId, int salesArrangementId, [FromBody] GenerateRetentionDocument.GenerateRetentionDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));

    /// <summary>
    /// Generování dokumentu pro Refixace nebo Individuální sdělení
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu pro Refixace nebo Individuální sdělení<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=A415BC33-46AF-40f9-B50C-5F7297DC0B26"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("/api/case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/refixation-document")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    public async Task GenerateRefixationDocument(long caseId, int salesArrangementId, [FromBody] GenerateRefixationDocument.GenerateRefixationDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));

    /// <summary>
    /// Generování dokumentu mimořádné splátky
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu mimořádné splátky<br /><br />
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=6ABBB7A4-03E8-4cd2-8D6B-FAD3C407AC20"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("/api/case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/extra-payment-document")]
    [Produces("application/json")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    public async Task GenerateExtraPaymentDocument(long caseId, int salesArrangementId, [FromBody] GenerateExtraPaymentDocument.GenerateExtraPaymentDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));
}
