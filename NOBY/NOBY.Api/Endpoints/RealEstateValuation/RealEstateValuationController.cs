﻿using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace NOBY.Api.Endpoints.RealEstateValuation;

[ApiController]
[Route("api/case")]
[ApiVersion(1)]
public sealed class RealEstateValuationController : ControllerBase
{
    /// <summary>
    /// Uložení dat pro místní šetření
    /// </summary>
    /// <remarks>
    /// Průběžné uložení konktaktů pro místní šetření pro pozdější použití.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=C6362E7C-7CC7-4b1c-86BA-D08BD84FFCA7"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/local-survey-data")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SaveLocalSurveyDetails(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] SaveLocalSurveyDetails.SaveLocalSurveyDetailsRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Nastavení typu Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Nastavení typu Ocenění. Slouží při možnosti uživatele vybrat typ Ocenění ze Standardu a DTS nebo při přechodu uživatele z Online ocenění na Standard.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=740D6F1C-FF1E-437e-96BD-01C6553C0F9A"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/valuation-type-id")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetValuationTypeId(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] SetValuationTypeId.SetValuationTypeIdRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Založení Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Operace založí nové Ocenění nemovitosti k danému case ID. Jde buď o založení Ocenění nemovitosti k objektu úvěru ze žádosti o úvěr nebo o manuální založení dalšího objektu zajištění.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=5A275F67-67AC-4851-8FE9-51F2B685B990"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Consumes("application/json")]
    [Produces("text/plain")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateRealEstateValuation(
        [FromRoute] long caseId, 
        [Required] [FromBody] CreateRealEstateValuation.CreateRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(request.InfuseId(caseId), cancellationToken);
        return Content(id.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    /// Smazání Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Smazání Ocenění konkrétní nemovitosti ještě před odesláním do ACV.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=6525504B-D598-4113-AA2A-846571769C40"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    /// Operace vrací detail Ocenění nemovitostí.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=7301EE13-E1C2-4795-A5FA-F8A646C4D057"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpGet("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(typeof(GetRealEstateValuationDetail.GetRealEstateValuationDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<GetRealEstateValuationDetail.GetRealEstateValuationDetailResponse> GetRealEstateValuationDetail(
        long caseId, 
        int realEstateValuationId, 
        CancellationToken cancellationToken) 
        => await _mediator.Send(new GetRealEstateValuationDetail.GetRealEstateValuationDetailRequest(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Získání seznamu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace vrací seznam všech ocenění nemovitostí k danému case ID. Seznam je sloučením ocenění objektů úvěru ze žádosti o úvěr a manuálně zadaných dalších objektů zajištění. Operace nevrací objednávky ocenění, které nevznikly v Noby.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=0DCD023D-ACF8-4744-B198-FE4FC2A84223"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [NobyAuthorize(UserPermissions.SALES_ARRANGEMENT_Access)]
    [HttpGet("{caseId:long}/real-estate-valuations")]
    [NobyAuthorizePreload(NobyAuthorizePreloadAttribute.LoadableEntities.Case)]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(typeof(List<Dto.RealEstateValuation.RealEstateValuationListItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<Dto.RealEstateValuation.RealEstateValuationListItem>> GetRealEstateValuationList(
        [FromRoute] long caseId, 
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRealEstateValuationList.GetRealEstateValuationListRequest(caseId), cancellationToken);

    /// <summary>
    /// Aktualizace detailu Ocenění nemovitostí
    /// </summary>
    /// <remarks>
    /// Operace edituje detail Ocenění nemovitostí.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=39883A18-AA29-4f7d-9E4E-BC2D5F81B115"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchDeveloperOnRealEstateValuation(
        [FromRoute] long caseId, 
        [FromRoute] int realEstateValuationId, 
        [Required] [FromBody] PatchDeveloperOnRealEstateValuation.PatchDeveloperOnRealEstateValuationRequest request, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Smazání již připojeného dokumentu LV
    /// </summary>
    /// <remarks>
    /// Smazání/odpojení již připojeného dokumentu listu vlastnictví (LV) od existujícího Ocenění nemovitosti.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=33180E91-2CB3-4d2f-B6AD-5841EC8A836F"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents/{deedOfOwnershipDocumentId:int}")]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int deedOfOwnershipDocumentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteDeedOfOwnershipDocument.DeleteDeedOfOwnershipDocumentRequest(caseId, realEstateValuationId, deedOfOwnershipDocumentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Spojení souboru přílohy s oceněním nemovitosti
    /// </summary>
    /// <remarks>
    /// Propojí uploadnutý soubor s oceněním a doplní k souboru popisek.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=B44C60EF-8521-4eec-9CE5-292C279DFE51"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <param name="attachments">Seznam souborů k propojení</param>
    /// <response code="200">Kolekce ID uploadovaných souborů vs. nových ID příloh</response>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/attachments")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(typeof(List<SaveRealEstateValuationAttachments.SaveRealEstateValuationAttachmentsResponseItem>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<SaveRealEstateValuationAttachments.SaveRealEstateValuationAttachmentsResponseItem>> SaveRealEstateValuationAttachments(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] List<SaveRealEstateValuationAttachments.SaveRealEstateValuationAttachmentsRequestItem> attachments,
        CancellationToken cancellationToken)
        => await _mediator.Send(new SaveRealEstateValuationAttachments.SaveRealEstateValuationAttachmentsRequest
        {
            CaseId = caseId,
            RealEstateValuationId = realEstateValuationId,
            Attachments = attachments,
        }, cancellationToken);

    /// <summary>
    /// Smazání přílohy ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Smazání již nahrané přílohy ocenění z databáze NOBY a ACV.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=4EFAC9BD-B78E-4219-A801-39E983D3EDAF"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpDelete("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/attachments/{realEstateValuationAttachmentId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano, RealEstateValuationStates.DoplneniDokumentu)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRealEstateValuationAttachment(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int realEstateValuationAttachmentId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteRealEstateValuationAttachment.DeleteRealEstateValuationAttachmentRequest(caseId, realEstateValuationId, realEstateValuationAttachmentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Uložení údajů identifikace nemovitosti k danému Ocenění nemovitosti
    /// </summary>
    /// <remarks>
    /// Uložení údajů identifikace nemovitosti k danému Ocenění nemovitosti.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=536CD827-3140-4a41-8AC2-AF6BB6700539"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    /// <response code="200">DeedOfOwnershipDocumentId: Noby ID daného záznamu. Určuje jednoznačnou kombinaci cremDeedOfOwnershipDocumentId a RealEstateValuationId (Noby Ocenění) pro případy simulování více možností žádostí s jednou nemovitostí.</response>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Neoceneno, RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<int> AddDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] AddDeedOfOwnershipDocument.AddDeedOfOwnershipDocumentRequest request,
        CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Získání typu Ocenění
    /// </summary>
    /// <remarks>
    /// Získání typu Ocenění provoláním systému ACV.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=0FE0440C-1614-47b0-8136-42BF508CE369"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/valuation-types")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<List<int>> GetRealEstateValuationTypes(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        CancellationToken cancellationToken)
        => await _mediator.Send(new GetRealEstateValuationTypes.GetRealEstateValuationTypesRequest(caseId, realEstateValuationId), cancellationToken);

    /// <summary>
    /// Aktualizace připojeného dokumentu LV
    /// </summary>
    /// <remarks>
    /// Operace edituje RealEstateIds na již připojeném dokumentu LV.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=22C309A5-60C5-4ca2-96DE-129CA8178977"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPut("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/deed-of-ownership-documents/{deedOfOwnershipDocumentId:int}")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDeedOfOwnershipDocument(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromRoute] int deedOfOwnershipDocumentId,
        [FromBody] UpdateDeedOfOwnershipDocument.UpdateDeedOfOwnershipDocumentRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId, deedOfOwnershipDocumentId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Předobjednávka online ocenění
    /// </summary>
    /// <remarks>
    /// Předobjednávka online ocenění.<br/><br/>
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=CA6B233C-6BE0-45ff-B5F6-F47F9A3ABA62"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/preorder-online")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation(RealEstateValuationStates.Rozpracovano)]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PreorderOnlineValuation(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] PreorderOnlineValuation.PreorderOnlineValuationRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Objednání ocenění
    /// </summary>
    /// <remarks>
    /// Objedná ocenění nemovitosti daného typu.
    /// 
    /// <a href="https://eacloud.ds.kb.cz/webea/index.php?m=1&amp;o=68C49245-60DD-48a4-9681-B328C52D86F4"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("{caseId:long}/real-estate-valuations/{realEstateValuationId:int}/order")]
    [NobyAuthorize(UserPermissions.REALESTATE_VALUATION_Manage)]
    [RealEstateValuationStateValidation]
    [SwaggerOperation(Tags = new[] { "Real Estate Valuation" })]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> OrderRealEstateValuation(
        [FromRoute] long caseId,
        [FromRoute] int realEstateValuationId,
        [FromBody] [Required] OrderRealEstateValuation.OrderRealEstateValuationRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(request.InfuseId(caseId, realEstateValuationId), cancellationToken);
        return NoContent();
    }

    private readonly IMediator _mediator;
    public RealEstateValuationController(IMediator mediator) => _mediator = mediator;
}
