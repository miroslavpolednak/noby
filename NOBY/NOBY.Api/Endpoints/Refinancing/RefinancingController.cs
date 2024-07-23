using Asp.Versioning;
using NOBY.Api.Endpoints.Refinancing.GetRefinancingParameters;
using Swashbuckle.AspNetCore.Annotations;
using NOBY.Api.Endpoints.Refinancing.GetMortgageRetention;
using NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;
using NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPayment;
using NOBY.Api.Endpoints.Refinancing.UpdateMortgageRefixation;
using NOBY.Api.Endpoints.Refinancing.GetAvailableFixedRatePeriods;
using NOBY.Api.Endpoints.Refinancing.GetMortgageExtraPaymentList;
using NOBY.Dto.Refinancing;
using NOBY.Infrastructure.Swagger;

namespace NOBY.Api.Endpoints.Refinancing;

[ApiController]
[Route("api/v{v:apiVersion}")]
[ApiVersion(1)]
public sealed class RefinancingController(IMediator _mediator) : ControllerBase
{
    /// <summary>
    /// Rozscestník mimořádných splátek 
    /// </summary>
    /// <remarks>
    /// Seznam nezrušených mimořádných splátek
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-extra-payment-list")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(List<GetMortgageExtraPaymentListResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=DFD62AED-36C2-49f0-A8A4-A245657D5A4C")]
    public async Task<List<GetMortgageExtraPaymentListResponse>> GetMortgageExtraPaymentList([FromRoute] long caseId)
        => await _mediator.Send(new GetMortgageExtraPaymentListRequest(caseId));

	/// <summary>
	/// Seznam dostupných délek fixací pro refixace
	/// </summary>
	/// <remarks>
	/// Seznam dostupných délek fixací pro refixace. Délky fixace jsou uvedeny v měsících.
	/// </remarks>
	[HttpGet("case/{caseId:long}/mortgage-refixation/available-fixed-rate-periods")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetAvailableFixedRatePeriodsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A4BCB0C7-506D-4edb-ADCD-66BA06A5D1DD")]
    public async Task<GetAvailableFixedRatePeriodsResponse> GetAvailableFixedRatePeriods([FromRoute] long caseId)
        => await _mediator.Send(new GetAvailableFixedRatePeriodsRequest(caseId));

	/// <summary>
	/// Vytvoří odpovědní kód
	/// </summary>
	/// <remarks>
	/// Uloží odpovědní kód ke case a k odeslání do BDP (Big Data Platform)
	/// </remarks>
	[HttpPut("case/{caseId:long}/mortgage/create-mortgage-response-code")]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=8251FF4A-E3C6-46c8-883E-D70C884A859D")]
    public async Task<IActionResult> CreateMortgageResponseCode([FromRoute] long caseId, [FromBody] CreateMortgageResponseCode.CreateMortgageResponseCodeRequest? request)
    {
        await _mediator.Send((request ?? new CreateMortgageResponseCode.CreateMortgageResponseCodeRequest()).InfuseId(caseId));
        return NoContent();
    }

    /// <summary>
    /// Uložení změny individuální cenotvorby a komentáře
    /// </summary>
    /// <remarks>
    /// Uloží změny ve výši slevy ze sazby včetně komentáře ke slevě. Dále uloží komentář k refixaci.
    /// </remarks>
    [HttpPut("case/{caseId:long}/update-mortgage-refixation")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(RefinancingLinkResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=D2F4DD7B-54B9-4c86-BEE0-8673DBBDE5DB")]
    public async Task<RefinancingLinkResult> UpdateMortgageRefixation([FromRoute] long caseId, [FromBody] UpdateMortgageRefixationRequest? request)
        => await _mediator.Send((request ?? new()).InfuseId(caseId));

    /// <summary>
    /// Detail mimořádné splátky
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném procesu mimořádné splátky.
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-extra-payment/{processId:long}")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageExtraPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=87469E98-536F-47b6-853F-077E5BC7FF4A")]
    public async Task<GetMortgageExtraPaymentResponse> GetMortgageExtraPayment([FromRoute] long caseId, [FromRoute] long processId)
        => await _mediator.Send(new GetMortgageExtraPaymentRequest(caseId, processId));

    /// <summary>
    /// Detail refixace
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném refixačním procesu.
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-refixation")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageRefixationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerConfluenceLink("https://wiki.kb.cz/display/HT/Refixace")]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=1EF978B1-265D-40d0-8B1C-B0E07D9D8031")]
    public async Task<GetMortgageRefixationResponse> GetMortgageRefixation([FromRoute] long caseId, [FromQuery] long? processId = null)
        => await _mediator.Send(new GetMortgageRefixationRequest(caseId, processId));

    /// <summary>
    /// Detail retence
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o vybraném retenčním procesu.
    /// </remarks>
    [HttpGet("case/{caseId:long}/mortgage-retention/{processId:long}")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetMortgageRetentionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=E0F5C17D-A6F5-4713-93DC-73E06D98AD09")]
    public async Task<GetMortgageRetentionResponse> GetMortgageRetention([FromRoute] long caseId, [FromRoute] long processId)
        => await _mediator.Send(new GetMortgageRetentionRequest(caseId, processId));

    /// <summary>
    /// Žádosti o změnu sazby
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací a žádostí ke změnám sazeb.
    /// </remarks>
    [HttpGet("case/{caseId:long}/refinancing-parameters")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [ProducesResponseType(typeof(GetRefinancingParametersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=EBF744FA-9F0F-421e-89F8-CFFFAEC76BB1")]
    public async Task<GetRefinancingParametersResponse> GetRefinancingParameters([FromRoute] long caseId)
        => await _mediator.Send(new GetRefinancingParametersRequest(caseId));

    /// <summary>
    /// Seznam možných platností sazby od
    /// </summary>
    /// <remarks>
    /// Vrátí kolekci možných platností sazby od pro konkrétní úvěr.
    /// </remarks>
    [HttpGet("case/{caseId:long}/interest-rates-valid-from")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [ProducesResponseType(typeof(GetInterestRatesValidFrom.GetInterestRatesValidFromResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=138F1225-E4D6-4a7c-B316-882C35CE2C74")]
    public async Task<GetInterestRatesValidFrom.GetInterestRatesValidFromResponse> GetInterestRatesValidFrom([FromRoute] long caseId)
        => await _mediator.Send(new GetInterestRatesValidFrom.GetInterestRatesValidFromRequest(caseId));

    /// <summary>
    /// Aktuální úroková sazba
    /// </summary>
    /// <remarks>
    /// Operace slouží k získání informací o aktuální úrokové sazbě.
    /// </remarks>
    [HttpGet("case/{caseId:long}/interest-rate")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [ProducesResponseType(typeof(GetInterestRate.GetInterestRateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=7C3EE41F-80E9-4bf2-A0CD-7E6E7F83D704")]
    public async Task<GetInterestRate.GetInterestRateResponse> GetInterestRate([FromRoute] long caseId)
        => await _mediator.Send(new GetInterestRate.GetInterestRateRequest(caseId));

    /// <summary>
    /// Sdělení nabídek refixací.
    /// </summary>
    /// <remarks>
    /// Smaže sdělené nabídky (pokud nejsou shodné s aktuálními) a označí aktuální paletu nabídek jako sdělenou.
    /// </remarks>
    [HttpPost("case/{caseId:long}/mortgage-refixation-offer-list/communicate")]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [SwaggerOperation(Tags = ["Modelace"])]
    [ProducesResponseType(typeof(RefinancingLinkResult), StatusCodes.Status200OK)]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=95CE2766-FF1F-4267-9873-177189302EDD")]
    public async Task<RefinancingLinkResult> CommunicateMortgageRefixation(long caseId) => 
        await _mediator.Send(new CommunicateMortgageRefixation.CommunicateMortgageRefixationRequest { CaseId = caseId});

    /// <summary>
    /// Generování dokumentu Retenčního dodatku
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu Retenčního dodatku
    /// </remarks>
    [HttpPost("case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/retention-document")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=5379DC03-6DFD-411c-9A7C-AB8203677FA9")]
    public async Task GenerateRetentionDocument(long caseId, int salesArrangementId, [FromBody] GenerateRetentionDocument.GenerateRetentionDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));

    /// <summary>
    /// Generování dokumentu pro Refixace nebo Individuální sdělení
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu pro Refixace nebo Individuální sdělení
    /// </remarks>
    [HttpPost("case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/refixation-document")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=A415BC33-46AF-40f9-B50C-5F7297DC0B26")]
    public async Task GenerateRefixationDocument(long caseId, int salesArrangementId, [FromBody] GenerateRefixationDocument.GenerateRefixationDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));

    /// <summary>
    /// Generování dokumentu mimořádné splátky
    /// </summary>
    /// <remarks>
    /// Operace slouží k vygenerování dokumentu mimořádné splátky
    /// </remarks>
    [HttpPost("case/{caseId:long}/sales-arrangement/{salesArrangementId:int}/extra-payment-document")]
    [Produces(MediaTypeNames.Application.Json)]
    [NobyAuthorize(UserPermissions.REFINANCING_Manage)]
    [NobyRequiredCaseStates(EnumCaseStates.InAdministration, EnumCaseStates.InDisbursement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Tags = ["Refinancing"])]
    [SwaggerEaDiagram("https://eacloud.ds.kb.cz/webea/index.php?m=1&o=6ABBB7A4-03E8-4cd2-8D6B-FAD3C407AC20")]
    public async Task GenerateExtraPaymentDocument(long caseId, int salesArrangementId, [FromBody] GenerateExtraPaymentDocument.GenerateExtraPaymentDocumentRequest request) => 
        await _mediator.Send(request.Infuse(caseId, salesArrangementId));
}
